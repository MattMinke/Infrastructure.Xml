using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace Infrastructure.Xml.Xsl
{
    /// <summary>
    /// A wrapper class that 
    /// organize and simplifies the public API on the <see cref="System.Xml.Xsl.XslCompiledTransform"/> class
    /// </summary>
    public class Transformer
    {
        /// <summary>
        /// field that holds the reference to the transformer this class wraps
        /// </summary>
        private XslCompiledTransform _transformer;

        /// <summary>
        /// field that holds the settings provided to during instantiation. 
        /// </summary>
        private readonly TransformerSettings _settings;


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
            _settings = new ReadOnlyTransformerSettings(settings);
        }

        /// <summary>
        /// Specifies the XSLT features to support during the execution of the XSLT style sheet.
        /// </summary>
        public TransformerSettings Settings => _settings;

        /// <summary>
        /// Exposes a read-only <see cref="System.Xml.XmlWriterSettings"/> object from the wrapped
        /// <see cref="System.Xml.Xsl.XslCompiledTransform"/> that contains the output information
        /// derived from the xsl:output element of the style sheet. This property can not be
        /// accessed until a Load method has been called.
        /// </summary>
        /// <remarks>
        /// https://web.archive.org/web/20140513023506/http://blogs.msdn.com/b/eriksalt/archive/2005/07/27/outputsettings.aspx
        /// </remarks>
        public XmlWriterSettings OutputSettings
        {
            get
            {
                // Make sure we have a transformer.
                if (_transformer == null)
                {
                    throw new InvalidOperationException(
                        "An overload of method 'Load' must be called before accessing property 'OutputSettings'");
                }

                // return the output setting from the transformer.
                return _transformer.OutputSettings;
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
            _transformer = new XslCompiledTransform(_settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            _transformer.Load(stylesheet, new XsltSettings(
                _settings.EnableDocumentFunction, _settings.EnableScript), _settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the file specified by the <see cref="System.IO.FileInfo"/> object
        /// </summary>
        /// <param name="stylesheet">the file path of the style sheet</param>
        public void Load(FileInfo stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            _transformer = new XslCompiledTransform(_settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            _transformer.Load(stylesheet.FullName, new XsltSettings(
                _settings.EnableDocumentFunction, _settings.EnableScript), _settings.Resolver);
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
            _transformer = new XslCompiledTransform(_settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            _transformer.Load(stylesheetUri, new XsltSettings(
                _settings.EnableDocumentFunction, _settings.EnableScript), _settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the <see cref="System.Xml.XmlReader"/>.
        /// </summary>
        /// <param name="stylesheet">A <see cref="System.Xml.XmlReader"/> containing the style sheet.</param>
        public void Load(XmlReader stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            _transformer = new XslCompiledTransform(_settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            _transformer.Load(stylesheet, new XsltSettings(
                _settings.EnableDocumentFunction, _settings.EnableScript), _settings.Resolver);
        }

        /// <summary>
        /// Loads and Compiles the style sheet contained in the <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stylesheet">A <see cref="System.IO.Stream"/> containing the style sheet.</param>
        public void Load(Stream stylesheet)
        {
            // Instantiate a new instance of the XslCompiledTransform class
            _transformer = new XslCompiledTransform(_settings.EnableDebug);

            // Load the style sheet using the provided settings and resolver.
            _transformer.Load(XmlReader.Create(stylesheet), new XsltSettings(
                _settings.EnableDocumentFunction, _settings.EnableScript), _settings.Resolver);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to an <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In the
        /// Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing the
        /// data to be transformed.
        /// </param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> to which you want to output. If the style sheet contains
        /// an xsl:output element, you should create the <see cref="System.Xml.XmlWriter"/> using the <see cref="System.Xml.XmlWriterSettings"/>
        /// object returned from the <see cref="OutputSettings"/> property.
        /// This ensures that the <see cref="System.Xml.XmlWriter"/> has the correct output settings.</param>
        /// <param name="documentResolver">The <see cref="System.Xml.XmlResolver"/> used to resolve the XSLT document() function. If this
        ///  is null, the document() function is not resolved.
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(IXPathNavigable input, XmlWriter result, XmlResolver documentResolver = null, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result, documentResolver ?? _settings.Resolver);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to an <see cref="System.IO.TextWriter"/>. 
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In the
        /// Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing the
        /// data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.TextWriter"/> to which you want to output.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(IXPathNavigable input, TextWriter result, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result);
        }
        
        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XPath.IXPathNavigable"/>
        /// object and outputs the results to an <see cref="System.IO.TextWriter"/>. 
        /// </summary>
        /// <param name="input">
        /// An object implementing the <see cref="System.Xml.XPath.IXPathNavigable"/> interface. In the
        /// Microsoft .NET Framework, this can be either an <see cref="System.Xml.XmlNode"/> (typically
        /// an <see cref="System.Xml.XmlDocument"/>), or an <see cref="System.Xml.XPath.XPathDocument"/> containing the
        /// data to be transformed.
        /// </param>
        /// <param name="result">The <see cref="System.IO.Stream"/> to which output will be written.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(IXPathNavigable input, Stream result, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to an <see cref="System.Xml.XmlWriter"/>. The <see cref="TransformArguments"/>
        /// provides additional run-time arguments and the <see cref="XmlResolver"/> resolves the XSLT
        /// document() function.
        /// </summary>
        /// <param name="input">An <see cref="System.Xml.XmlReader"/> containing the input document.</param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> to which you want to output. If the style sheet contains
        /// an xsl:output element, you should create the <see cref="System.Xml.XmlWriter"/> using the <see cref="System.Xml.XmlWriterSettings"/>
        /// object returned from the <see cref="OutputSettings"/> property.
        /// This ensures that the <see cref="System.Xml.XmlWriter"/> has the correct output settings.
        /// </param>
        /// <param name="documentResolver">The <see cref="System.Xml.XmlResolver"/> used to resolve the XSLT document() function. If this
        ///  is null, the document() function is not resolved.
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(XmlReader input, XmlWriter result, XmlResolver documentResolver = null, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result, documentResolver ?? Settings.Resolver);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to an <see cref="System.IO.Stream"/>. The <see cref="TransformArguments"/>
        /// provides additional run-time arguments.
        /// </summary>
        /// <param name="input">An <see cref="System.Xml.XmlReader"/> containing the input document.</param>
        /// <param name="result">The <see cref="System.IO.Stream"/> to which you want to output.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(XmlReader input, Stream result, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the <see cref="System.Xml.XmlReader"/>
        /// object and outputs the results to an <see cref="System.IO.TextWriter"/>. The <see cref="TransformArguments"/>
        /// provides additional run-time arguments.
        /// </summary>
        /// <param name="input">An <see cref="System.Xml.XmlReader"/> containing the input document.</param>
        /// <param name="result">The <see cref="System.IO.TextWriter"/> to which you want to output.</param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(XmlReader input, TextWriter result, TransformArguments arguments = null)
        {
            _transformer.Transform(input, arguments?.CreateArguments(), result);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the URI and outputs
        /// the results to an <see cref="System.Xml.XmlWriter"/>. The <see cref="TransformArguments"/> provides
        /// additional run-time arguments.
        /// </summary>
        /// <param name="inputUri">The URI of the input document.</param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> to which you want to output. If the style sheet contains
        /// an xsl:output element, you should create the <see cref="System.Xml.XmlWriter"/> using the <see cref="System.Xml.XmlWriterSettings"/>
        /// object returned from the <see cref="OutputSettings"/> property.
        /// This ensures that the <see cref="System.Xml.XmlWriter"/> has the correct output settings.
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(string inputUri, XmlWriter result, TransformArguments arguments = null)
        {
            _transformer.Transform(inputUri, arguments?.CreateArguments(), result);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the URI and outputs
        /// the results to an <see cref="System.IO.TextWriter"/>. The <see cref="TransformArguments"/> provides
        /// additional run-time arguments.
        /// </summary>
        /// <param name="inputUri">The URI of the input document.</param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> to which you want to output. If the style sheet contains
        /// an xsl:output element, you should create the <see cref="System.Xml.XmlWriter"/> using the <see cref="System.Xml.XmlWriterSettings"/>
        /// object returned from the <see cref="OutputSettings"/> property.
        /// This ensures that the <see cref="System.Xml.XmlWriter"/> has the correct output settings.
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(string inputUri, TextWriter result, TransformArguments arguments = null)
        {
            _transformer.Transform(inputUri, arguments?.CreateArguments(), result);
        }

        /// <summary>
        /// Executes the transform using the input document specified by the URI and outputs
        /// the results to an <see cref="System.IO.TextWriter"/>. The <see cref="TransformArguments"/> provides
        /// additional run-time arguments.
        /// </summary>
        /// <param name="inputUri">The URI of the input document.</param>
        /// <param name="result">
        /// The <see cref="System.Xml.XmlWriter"/> to which you want to output. If the style sheet contains
        /// an xsl:output element, you should create the <see cref="System.Xml.XmlWriter"/> using the <see cref="System.Xml.XmlWriterSettings"/>
        /// object returned from the <see cref="OutputSettings"/> property.
        /// This ensures that the <see cref="System.Xml.XmlWriter"/> has the correct output settings.
        /// </param>
        /// <param name="arguments">An optional parameter that provide additional input and functionality to the XSLT transformer</param>
        /// <exception cref="System.ArgumentException">The input or result value is null.</exception>
        /// <exception cref="System.Xml.Xsl.XsltException">There was an error executing the XSLT transform.</exception>
        public void Transform(string inputUri, Stream result, TransformArguments arguments = null)
        {
            _transformer.Transform(inputUri, arguments?.CreateArguments(), result);
        }
    }
}