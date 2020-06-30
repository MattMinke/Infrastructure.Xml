using System;
using System.Collections.Concurrent;
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
    public class XmlSerializerFactory : IXmlSerializerFactory
    {
        private readonly ConcurrentDictionary<SerializerWithRootKey, XmlSerializer> _serializerWithRoot;
        private readonly ConcurrentDictionary<SerializerWithOverridesKey, XmlSerializer> _serializerWithOverrides;

        /// <summary>
        /// Initializes a new instance of <see cref="XmlSerializerFactory"/>
        /// </summary>
        public XmlSerializerFactory()
        {
            _serializerWithRoot = new ConcurrentDictionary<SerializerWithRootKey, XmlSerializer>(SerializerWithRootKeyComparer.Instance);
            _serializerWithOverrides = new ConcurrentDictionary<SerializerWithOverridesKey, XmlSerializer>(SerializerWithOverridesKeyComparer.Instance);
        }

        /// <summary>
        /// Creates a XmlSerializer for <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type a XmlSerializer is requested for.</typeparam>
        /// <returns>a non-null XmlSerializer</returns>
        public XmlSerializer Create<T>()
        {
            // No caching required
            return new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// Creates a XmlSerializer for <paramref name="t"/>
        /// </summary>
        /// <param name="t">The type a XmlSerializer is requested for.</param>
        /// <returns>a non-null XmlSerializer</returns>
        public XmlSerializer Create(Type t)
        {
            // No caching required
            if (t == null) { throw new ArgumentNullException("t"); }
            return new XmlSerializer(t);
        }

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
        public XmlSerializer Create<T>(XmlRootAttribute root)
        {
            return Create(typeof(T), root);
        }

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
        public XmlSerializer Create(Type t, XmlRootAttribute root)
        {
            if (t == null) { throw new ArgumentNullException("t"); }
            if (root == null)
            {
                return Create(t);
            }
            return _serializerWithRoot.GetOrAdd(
                new SerializerWithRootKey(t, root),
                key => new XmlSerializer(key.Type, key.Root));
        }



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
        public XmlSerializer Create<T>(string cacheKey, XmlAttributeOverrides overrides)
        {
            return Create(typeof(T), cacheKey, overrides);
        }

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
        public XmlSerializer Create(Type t, string cacheKey, XmlAttributeOverrides overrides)
        {
            if (t == null) { throw new ArgumentNullException("t"); }
            if (overrides == null)
            {
                return Create(t);
            }
            if (cacheKey == null) { throw new ArgumentNullException("cacheKey"); }

            return _serializerWithOverrides.GetOrAdd(
                new SerializerWithOverridesKey(t, cacheKey),
                key => new XmlSerializer(key.Type, overrides));
        }

        /// <summary>
        /// A helper structure used as a key when caching XmlSerializers that have a RootAttribute override.
        /// </summary>
        private struct SerializerWithRootKey
        {
            private readonly Type _type;
            private readonly XmlRootAttribute _root;

            public SerializerWithRootKey(Type type, XmlRootAttribute root)
            {
                _type = type;
                _root = root;
            }

            public Type Type { get { return _type; } }

            public XmlRootAttribute Root { get { return _root; } }

            public override bool Equals(object obj)
            {
                if (obj is SerializerWithRootKey)
                {
                    return SerializerWithRootKeyComparer.Instance.Equals(this, (SerializerWithRootKey)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return SerializerWithRootKeyComparer.Instance.GetHashCode(this);
            }

        }

        /// <summary>
        /// A helper structure used as a key when caching XmlSerializers that have a collection of overrides.
        /// </summary>
        private struct SerializerWithOverridesKey
        {
            private Type _type;
            private string _key;

            public SerializerWithOverridesKey(Type type, string key)
            {
                _type = type;
                _key = key;
            }

            public Type Type { get { return _type; } }

            public string Key { get { return _key; } }

            public override bool Equals(object obj)
            {
                if (obj is SerializerWithOverridesKey)
                {
                    return SerializerWithOverridesKeyComparer.Instance.Equals(this, (SerializerWithOverridesKey)obj);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return SerializerWithOverridesKeyComparer.Instance.GetHashCode(this);
            }
        }

        /// <summary>
        /// An equality comparer for <see cref="SerializerWithRootKey"/>
        /// </summary>
        private class SerializerWithRootKeyComparer : EqualityComparer<SerializerWithRootKey>
        {
            private static readonly SerializerWithRootKeyComparer _instance = new SerializerWithRootKeyComparer();

            public static SerializerWithRootKeyComparer Instance
            {
                get { return _instance; }
            }

            public sealed override bool Equals(SerializerWithRootKey x, SerializerWithRootKey y)
            {
                return x.Type == y.Type &&
                    (
                        (x.Root == null && y.Root == null) ||
                        (
                            x.Root != null && y.Root != null &&
                            string.Equals(x.Root.DataType, y.Root.DataType) &&
                            string.Equals(x.Root.ElementName, y.Root.ElementName) &&
                            x.Root.IsNullable == y.Root.IsNullable &&
                            string.Equals(x.Root.Namespace, y.Root.Namespace)
                        )
                    );
            }

            public sealed override int GetHashCode(SerializerWithRootKey obj)
            {
                int hash = 7;
                unchecked
                {
                    if (obj.Root != null)
                    {
                        hash ^= obj.Root.GetHashCode();
                    }
                    hash ^= obj.Type.GetHashCode();
                }
                return hash;
            }
        }

        /// <summary>
        /// An equality comparer for <see cref="SerializerWithOverridesKey"/>
        /// </summary>
        private class SerializerWithOverridesKeyComparer : EqualityComparer<SerializerWithOverridesKey>
        {
            private static readonly SerializerWithOverridesKeyComparer _instance = new SerializerWithOverridesKeyComparer();

            public static SerializerWithOverridesKeyComparer Instance
            {
                get { return _instance; }
            }

            public sealed override bool Equals(SerializerWithOverridesKey x, SerializerWithOverridesKey y)
            {
                return x.Type == y.Type && string.Equals(x.Key, y.Key);
            }

            public sealed override int GetHashCode(SerializerWithOverridesKey obj)
            {
                int hash = 7;
                unchecked
                {
                    if (obj.Key != null)
                    {
                        hash ^= obj.Key.GetHashCode();
                    }
                    hash ^= obj.Type.GetHashCode();
                }
                return hash;
            }
        }
    }
}
