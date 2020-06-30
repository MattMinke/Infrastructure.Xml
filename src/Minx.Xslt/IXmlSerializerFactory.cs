using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Infrastructure.Xml
{
    /// <summary>
    /// A factory that creates XmlSerializer and caches them when necessary to prevent memory leaks
    /// </summary>
    public interface IXmlSerializerFactory
    {
        /// <summary>
        /// Creates a XmlSerializer for <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type a XmlSerializer is requested for.</typeparam>
        /// <returns>a non-null XmlSerializer</returns>
        XmlSerializer Create<T>();

        /// <summary>
        /// Creates a XmlSerializer for <paramref name="t"/>
        /// </summary>
        /// <param name="t">The type a XmlSerializer is requested for.</param>
        /// <returns>a non-null XmlSerializer</returns>
        XmlSerializer Create(Type t);

        /// <summary>
        /// Creates a XmlSerializer for <typeparamref name="T"/> and caches it using the provided <paramref name="root"/> object
        /// </summary>
        /// <typeparam name="T">The type a XmlSerializer is requested for.</typeparam>
        /// <param name="root">Provides override information for the root xml attribute.</param>
        /// <returns>A non-null cached XmlSerializer</returns>
        /// <remarks>
        /// There is a memory leak in the XmlSerializer constructor that takes a <see cref="XmlRootAttribute"/>. 
        /// By caching the XmlSerializer We can mitigate the memory leak issue.
        /// </remarks>
        XmlSerializer Create<T>(XmlRootAttribute root);

        /// <summary>
        /// Creates a XmlSerializer for <paramref name="t"/> and caches it using the provided <paramref name="root"/> object
        /// </summary>
        /// <param name="t">The type a XmlSerializer is requested for.</param>
        /// <param name="root">Provides override information for the root xml attribute.</param>
        /// <returns>A non-null cached XmlSerializer</returns>
        /// <remarks>
        /// There is a memory leak in the XmlSerializer constructor that takes a <see cref="XmlRootAttribute"/>. 
        /// By caching the XmlSerializer We can mitigate the memory leak issue.
        /// </remarks>
        XmlSerializer Create(Type t, XmlRootAttribute root);

        /// <summary>
        /// Creates a XmlSerializer for <typeparamref name="T"/> and caches it using the provided <paramref name="cacheKey"/>
        /// </summary>
        /// <typeparam name="T">The type a XmlSerializer is requested for</typeparam>
        /// <param name="cacheKey">A key that will be used when caching the requested XmlSerializer.</param>
        /// <param name="overrides">A collection of overrides that should be used when generating the XmlSerializer</param>
        /// <returns>A non-null cached XmlSerializer</returns>
        /// <remarks>
        /// There is a memory leak in the XmlSerializer constructor that takes a <see cref="XmlAttributeOverrides"/>. 
        /// This factory method takes a <paramref name="cacheKey"/> that will be used to cache instances of XmlSerializers 
        /// created in this manner. Its Important to reuse the XmlSerializer as much as possible (pass the same cacheKey)
        /// otherwise there will be no benefit when using this method.
        /// </remarks>
        XmlSerializer Create<T>(string cacheKey, XmlAttributeOverrides overrides);

        /// <summary>
        /// Creates a XmlSerializer for <paramref name="t"/> and caches it using the provided <paramref name="cacheKey"/>
        /// </summary>
        /// <param name="t">The type a XmlSerializer is requested for</param>
        /// <param name="cacheKey">A key that will be used when caching the requested XmlSerializer.</param>
        /// <param name="overrides">A collection of overrides that should be used when generating the XmlSerializer</param>
        /// <returns>A non-null cached XmlSerializer</returns>
        /// <remarks>
        /// There is a memory leak in the XmlSerializer constructor that takes a <see cref="XmlAttributeOverrides"/>. 
        /// This factory method takes a <paramref name="cacheKey"/> that will be used to cache instances of XmlSerializers 
        /// created in this manner. Its Important to reuse the XmlSerializer as much as possible (pass the same cacheKey)
        /// otherwise there will be no benefit when using this method.
        /// </remarks>
        XmlSerializer Create(Type t, string cacheKey, XmlAttributeOverrides overrides);
    }
}
