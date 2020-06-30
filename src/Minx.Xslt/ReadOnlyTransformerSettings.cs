using System;
using System.Xml;

namespace Infrastructure.Xml.Xsl
{
    /// <summary>
    /// A read only version of the TransformerSettings class. Any Attempt to set a property will throw a <see cref="System.NotSupportedException"/>
    /// </summary>
    public class ReadOnlyTransformerSettings : TransformerSettings
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyTransformerSettings"/>
        /// </summary>
        /// <param name="settings">The settings to make read only</param>
        public ReadOnlyTransformerSettings(TransformerSettings settings)
        {
            this._settings = settings;
        }


        private TransformerSettings _settings;

        /// <summary>
        /// Gets the value indicating whether debugging the style sheet with a debugger is possible.
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
        public override bool EnableDebug
        {
            get { return this._settings.EnableDebug; }
            set { throw new NotSupportedException("Setting of property 'EnableDebug' is not supported"); }
        }
        /// <summary>
        /// Gets the value indicating whether to enable support for the XSLT document() function.
        /// </summary>
        public override bool EnableDocumentFunction
        {
            get { return this._settings.EnableDocumentFunction; }
            set { throw new NotSupportedException("Setting of property 'EnableDocumentFunction' is not supported"); }
        }

        /// <summary>
        /// Gets the value indicating whether to enable support for embedded script blocks.
        /// </summary>
        public override bool EnableScript
        {
            get { return this._settings.EnableScript; }
            set { throw new NotSupportedException("Setting of property 'EnableScript' is not supported"); }
        }

        /// <summary>
        /// Gets the <see cref="System.Xml.XmlResolver"/> object that should be used to resolve external XML resources
        /// </summary>
        public override XmlResolver Resolver
        {
            get { return this._settings.Resolver; }
            set { throw new NotSupportedException("Setting of property 'Resolver' is not supported"); }
        }
    }

}
