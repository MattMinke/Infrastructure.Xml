using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace Minx.Xslt
{
    /// <summary>
    /// A wrapper class that 
    /// organize and simplifies the public API on the <see cref="System.Xml.Xsl.XslCompiledTransform"/> class
    /// </summary>
    public class Transformer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">A <see cref="Minx.Xslt.TransformerSettings"/> object that will be used to configure the transformer</param>
        public Transformer(TransformerSettings settings = null)
        {
            // If a settings object was not provided instantiate one now.
            if (settings == null) { settings = new TransformerSettings(); }

            // XmlUrlResolver is the default resolver for all classes in the System.Xml namespace. 
            // Because we are exposing this object we need to make sure one is set.
            settings.Resolver = settings.Resolver ?? new XmlUrlResolver();

            // wrap the provided settings object with a read only version.
            this.Settings = new ReadOnlyTransformerSettings(settings);
        }


        /// <summary>
        /// protected field that holds the reference to the transformer this class wraps
        /// </summary>
        protected XslCompiledTransform transformer;

        /// <summary>
        /// Specifies the XSLT features to support during the execution of the XSLT style sheet.
        /// </summary>
        public TransformerSettings Settings { get; private set; }

        /// <summary>
        /// Exposes a read-only <see cref="System.Xml.XmlWriterSettings"/> object from the wrapped
        /// <see cref="System.Xml.Xsl.XslCompiledTransform"/> that contains the output information
        /// derived from the xsl:output element of the style sheet. This property can not be
        /// accessed until a Load method has been called.
        /// </summary>
        /// <remarks>
        /// http://blogs.msdn.com/b/eriksalt/archive/2005/07/27/outputsettings.aspx
        /// </remarks>
        public XmlWriterSettings OutputSettings
        {
            get
            {
                // Make sure we have a transformer.
                if (this.transformer == null)
                {
                    throw new MethodCallRequiredException("Load",
                        "Method Load must be called before accessing property OutputSettings");
                }

                // return the output setting from the transformer.
                return this.transformer.OutputSettings;
            }
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the <see cref="System.Xml.XPath.IXPathNavigable"/> object
        /// </summary>
        /// <param name="stylesheet">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In
        /// the Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing
        /// the style sheet.
        /// </param>
        public void Load(System.Xml.XPath.IXPathNavigable stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            this.transformer = new XslCompiledTransform(this.Settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            this.transformer.Load(stylesheet, new XsltSettings(
                this.Settings.EnableDocumentFunction, this.Settings.EnableScript), this.Settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the file specified by the <see cref="System.IO.FileInfo"/> object
        /// </summary>
        /// <param name="stylesheet">the file path of the style sheet</param>
        public void Load(FileInfo stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            this.transformer = new XslCompiledTransform(this.Settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            this.transformer.Load(stylesheet.ToString(), new XsltSettings(
                this.Settings.EnableDocumentFunction, this.Settings.EnableScript), this.Settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet located at the specified URI 
        /// </summary>
        /// <param name="stylesheet">
        /// The URI of the style sheet. The URIs that are supported depends on the XmlResolver that was provided to this class during instantiation. 
        /// If a XmlResolver was not provided then <see cref="System.Xml.XmlUrlResolver"/> will be used.
        /// </param>
        public void Load(string stylesheetUri)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            this.transformer = new XslCompiledTransform(this.Settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            this.transformer.Load(stylesheetUri, new XsltSettings(
                this.Settings.EnableDocumentFunction, this.Settings.EnableScript), this.Settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the <see cref="System.Xml.XmlReader"/>.
        /// </summary>
        /// <param name="stylesheet">A <see cref="System.Xml.XmlReader"/> containing the style sheet.</param>
        public void Load(XmlReader stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            this.transformer = new XslCompiledTransform(this.Settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            this.transformer.Load(stylesheet, new XsltSettings(
                this.Settings.EnableDocumentFunction, this.Settings.EnableScript), this.Settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stylesheet">A <see cref="System.IO.Stream"/> containing the style sheet.</param>
        public void Load(Stream stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            this.transformer = new XslCompiledTransform(this.Settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            this.transformer.Load(XmlReader.Create(stylesheet), new XsltSettings(
                this.Settings.EnableDocumentFunction, this.Settings.EnableScript), this.Settings.Resolver);
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to the provided <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In
        /// the Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing
        /// the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.Stream"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(IXPathNavigable input, System.IO.Stream result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to the provided <see cref="System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In
        /// the Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing
        /// the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.TextWriter"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(IXPathNavigable input, System.IO.TextWriter result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to the provided <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In
        /// the Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing
        /// the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.Text.StringBuilder"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(IXPathNavigable input, System.Text.StringBuilder result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to the provided <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In
        /// the Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing
        /// the data to be transformed.
        /// </param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> that will contain the result of the transformation.
        /// When creating the <see cref="System.Xml.XmlWriter"/> that is provided to this method use the 
        /// <see cref="System.Xml.XmlWriterSettings"/> provided by the OutputSettings property of this instance
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(IXPathNavigable input, XmlWriter result, TransformArguments arguments = null)
        {
            this.transformer.Transform(input, arguments != null ? arguments.CreateArguments() : null, result, this.Settings.Resolver);
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to the provided <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.Xml.XmlReader"/> containing the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.Stream"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(XmlReader input, System.IO.Stream result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to the provided <see cref="System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.Xml.XmlReader"/> containing the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.TextWriter"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(XmlReader input, System.IO.TextWriter result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to the provided <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.Xml.XmlReader"/> containing the data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.Text.StringBuilder"/> that will contain the result of the transformation.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(XmlReader input, System.Text.StringBuilder result, TransformArguments arguments = null)
        {
            using (XmlWriter writer = XmlWriter.Create(result, this.transformer.OutputSettings))
            {
                this.Transform(input, writer, arguments);
                writer.Flush();
            }
        }

        /// <summary>
        /// Executes the loaded XSLT using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to the provided <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.Xml.XmlReader"/> containing the data to be transformed.
        /// </param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> that will contain the result of the transformation. 
        /// When creating the <see cref="System.Xml.XmlWriter"/> that is provided to this method use the 
        /// <see cref="System.Xml.XmlWriterSettings"/> provided by the OutputSettings property of this instance
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        public void Transform(XmlReader input, XmlWriter result, TransformArguments arguments = null)
        {
            this.transformer.Transform(input, arguments != null ? arguments.CreateArguments() : null, result, this.Settings.Resolver);
        }
    }
}