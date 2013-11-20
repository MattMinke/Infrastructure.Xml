using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;

namespace WebApplication.ExtensionFunctions
{
    public class MathExtensions : Minx.Xslt.IXsltFunctionProvider
    {
        /// <summary>
        /// The uniform resource identifier used to refer to the XSLT extension object. Value: "urn:Math"
        /// </summary>
        public string NamespaceUri
        {
            get { return "urn:Math"; }
        }


        private Random randomNumberGenerator = new Random();

        /// <summary>
        /// computes a remainder given two numbers
        /// </summary>
        /// <param name="dividend">the number that should be divided</param>
        /// <param name="divisor">the number the <paramref name="dividend"/> should by divided by</param>
        /// <returns>The remainder</returns>
        public int Modulus(int dividend, int divisor)
        {
            return dividend % divisor;
        }

        /// <summary>
        /// Creates an XML fragment containing a list of random numbers of length <paramref name="count"/> 
        /// between <paramref name="min"/> and <paramref name="max"/>
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned </param>
        /// <param name="max">
        /// The exclusive upper bound of the random number returned. <paramref name="max"/> 
        /// must be greater than or equal to <see cref="min"/>
        /// </param>
        /// <param name="count">
        /// the number of random numbers to return
        /// </param>
        /// <returns>
        /// An XML fragment containing the random numbers.
        /// </returns>
        public XPathNodeIterator GetRandomNumbers(int min, int max, int count)
        {
            // create a new xml document
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            // create a xml writer that will append to the xml document.
            using (System.Xml.XmlWriter writer = document.CreateNavigator().AppendChild())
            {
                // Write the start of the document.
                writer.WriteStartDocument();
                {
                    // write <Numbers>
                    writer.WriteStartElement("Numbers");
                    {
                        for (int i = 0; i < count; i++)
                        {
                            int randomNumber = randomNumberGenerator.Next(min, max);

                            // write <Number>{randomNumber}</Number>
                            writer.WriteElementString("Number", randomNumber.ToString());
                        }
                    }
                    writer.WriteEndElement();// write </Numbers>
                }
                writer.WriteEndDocument(); // Write the end of the document.

                // flush the writer before disposing.
                writer.Flush();
            }

            // create a XPathNodeIterator from the xml document, and select the whole document
            return document.CreateNavigator().Select(".");
        }

    }
}