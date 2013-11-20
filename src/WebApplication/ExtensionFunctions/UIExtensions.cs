using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.IO;

namespace WebApplication.ExtensionFunctions
{
    public class UIExtensions : Minx.Xslt.IXsltFunctionProvider
    {
        /// <summary>
        /// The uniform resource identifier used to refer to the XSLT extension object. Value: "urn:UI"
        /// </summary>
        public string NamespaceUri
        {
            get { return "urn:UI"; }
        }

        public string StringifyXml(System.Xml.XPath.XPathNodeIterator xml)
        {
            var sb = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(sb))
            using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
            {
                xmlWriter.Formatting = Formatting.Indented;

                while (xml.MoveNext())
                {
                    xml.Current.WriteSubtree(xmlWriter);
                }

                xmlWriter.Flush();
                stringWriter.Flush();
            }

            return sb.ToString();
        }

        
    }
}