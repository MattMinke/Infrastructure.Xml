using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Infrastructure.Xml.Xsl
{
    /// <summary>
    /// Specifies the XSLT features to support during the execution of a XSLT style sheet.
    /// </summary>
    public class TransformerSettings
    {
        /// <summary>
        /// Constructor. Initializes a new instance of this class with default settings. 
        /// </summary>
        public TransformerSettings()
        {
        }

        /// <summary>
        /// Constructor. Initializes a new instance of this class with the specified values.
        /// </summary>
        /// <param name="enableDocumentFunction"> true to enable support for the XSLT document() function; otherwise, false.</param>
        /// <param name="enableScript">true to enable support for embedded scripts blocks; otherwise, false.</param>
        /// <param name="enableDebug">true to enable debugging of a style sheet with a debugger; otherwise, false.</param>
        /// <param name="resolver">
        /// The <see cref="System.Xml.XmlResolver"/> used to resolve external resources.
        /// </param>
        public TransformerSettings(bool enableDocumentFunction, bool enableScript, bool enableDebug, XmlResolver resolver)
        {
            // Set all of the properties.
            this.EnableDocumentFunction = enableDocumentFunction;
            this.EnableScript = enableScript;
            this.EnableDebug = enableDebug;
            this.Resolver = resolver;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable support for the XSLT document() function.
        /// </summary>
        public virtual bool EnableDocumentFunction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable support for embedded script blocks.
        /// </summary>
        public virtual bool EnableScript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debugging the style sheet with a debugger is possible.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/ms163418%28v=vs.110%29.aspx
        /// The following conditions must be met in order to step into the code and debug the style sheet:
        /// 1) The enableDebug parameter is set to true.
        /// 2) The style sheet is passed to the Load method either as a URI, or an implementation of the XmlReader class that implements the IXmlLineInfo interface. The IXmlLineInfo interface is implemented on all text-parsing XmlReader objects.
        /// In other words, if the style sheet is loaded using an IXPathNavigable object, such as an XmlDocument or XPathDocument, or an XmlReader implementation that does not implement the IXmlLineInfo interface, you cannot debug the style sheet.
        /// 3) The XmlResolver used to load the style sheet is a file-based XmlResolver, such as the XmlUrlResolver (this is the default XmlResolver used by the XslCompiledTransform class).
        /// 4) The style sheet is located on the local machine or on the intranet.
        /// </remarks>
        public virtual bool EnableDebug { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="System.Xml.XmlResolver"/> object that should be used to resolve external XML resources
        /// </summary>
        public virtual XmlResolver Resolver { get; set; }

    }

}
