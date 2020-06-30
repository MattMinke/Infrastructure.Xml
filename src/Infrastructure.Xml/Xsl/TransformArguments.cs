using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;

namespace Infrastructure.Xml.Xsl
{
    /// <summary>
    /// A settings class that is used by the <see cref="Minx.Xslt.Transformer.Transform"/> method. 
    /// It provides parameters and extension objects to an XSLT. 
    /// </summary>
    public class TransformArguments
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TransformArguments()
        {
            this.FunctionProviders = new List<IXsltFunctionProvider>();
            this.Parameters = new ParameterCollection();
        }

        /// <summary>
        /// A collection of <see cref="Minx.Xslt.IXsltFunctionProvider"/>s.
        /// </summary>
        public ICollection<IXsltFunctionProvider> FunctionProviders { get; private set; }

        /// <summary>
        /// A collection of parameters that can be passed to an XSLT.
        /// </summary>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Builder method that creates a <see cref="System.Xml.Xsl.XsltArgumentList"/>
        /// </summary>
        /// <returns></returns>
        internal XsltArgumentList CreateArguments()
        {
            // instantiate a new instance of the result.
            XsltArgumentList result = new XsltArgumentList();

            // populate the result with all the parameters.
            foreach (var item in this.Parameters)
            {
                result.AddParam(item.Key, String.Empty, item.Value);
            }

            // populate the result with all the function providers.
            foreach (var item in this.FunctionProviders)
            {
                result.AddExtensionObject(item.NamespaceUri, item);
            }

            //return the populated XsltArgumentList
            return result;
        }


        /// <summary>
        /// Removes duplicates of the same function provider from the provided collection. If a two or more 
        /// different function providers are using the same namespace, then an exception is thrown.
        /// </summary>
        /// <param name="functionProviders">The function provider to remove duplicates from.</param>
        /// <returns></returns>
        private static List<IXsltFunctionProvider> Normalize(IEnumerable<IXsltFunctionProvider> functionProviders)
        {
            var results = new List<IXsltFunctionProvider>();

            // group all the function providers by Namespace.
            foreach (var group in functionProviders.Where(o => o != null).GroupBy(o => o.NamespaceUri))
            {
                var first = group.First();

                // if there is more than one function provider for a namespace
                // check to see if all the function providers are of the same type.
                // If not the same namespace is being used by multiple function providers.
                if (group.Count() > 1)
                {
                    Type t = first.GetType();
                    if (!group.All(o => o.GetType() == t))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("The namespace for each {0} is expected to be unique. The namespace '{1}' is being used by the following types.",
                            nameof(IXsltFunctionProvider), group.Key);

                        foreach (Type type in group.Select(o => o.GetType()).Distinct())
                        {
                            sb.AppendLine();
                            sb.Append(type.FullName);
                        }
                        throw new XsltException(sb.ToString());
                    }
                }
                results.Add(first);
            }
            return results;
        }

    }
}
