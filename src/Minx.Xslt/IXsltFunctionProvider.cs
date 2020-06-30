using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Xml.Xsl
{
    /// <summary>
    /// Public methods exposed by implementers of this interface can be called from inside an XSLT transformer. 
    /// For these public methods to be utilized they must meet the following criteria:
    /// 1) Method parameters must be of type <see cref="System.String"/>, <see cref="System.Int32"/>,
    /// <see cref="System.Boolean"/>, or <see cref="System.Xml.XPath.XPathNodeIterator"/>.
    /// 2) The use of the params keyword, which allows an unspecified number of parameters to be passed, is not supported.
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/system.xml.xsl.xsltargumentlist.addextensionobject%28v=vs.110%29.aspx
    /// </remarks>
    public interface IXsltFunctionProvider
    {
        /// <summary>
        /// The uniform resource identifier used to refer to the XSLT extension object
        /// </summary>
        string NamespaceUri { get; }
    }

}
