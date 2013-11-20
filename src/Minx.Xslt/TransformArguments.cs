using System;
using System.Collections.Generic;
using System.Xml.Xsl;

namespace Minx.Xslt
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

    }
}
