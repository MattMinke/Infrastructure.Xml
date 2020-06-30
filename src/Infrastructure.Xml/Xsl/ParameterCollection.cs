using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Infrastructure.Xml.Xsl
{

    /// <summary>
    /// A collection class that provides corresponding add methods for the supported WC3 types that can be passed as parameters
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/system.xml.xsl.xsltargumentlist.addparam%28v=vs.110%29.aspx
    /// </remarks>
    public class ParameterCollection : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// A backing collection that will hold the data of this class
        /// </summary>
        private Dictionary<string, object> _parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParameterCollection()
        {
            // Create a new instance of the backing collection
            this._parameters = new Dictionary<string, object>();
        }


        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, string value)
        {
            this._parameters.Add(name, value);
        }
        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, bool value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, int value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, double value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, params XPathNavigator[] value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="name">The name to associate with the parameter</param>
        /// <param name="value">The value to associate with the parameter</param>
        public void Add(string name, XPathNodeIterator value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Removes a parameter associated with the given name
        /// </summary>
        /// <param name="name">the name of the parameter to remove</param>
        public void RemoveParameter(string name)
        {
            this._parameters.Remove(name);
        }

        /// <summary>
        /// Clears all parameters from the collection
        /// </summary>
        public void Clear()
        {
            this._parameters.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the parameters in the collection.
        /// </summary>
        /// <returns>
        /// Returns an enumerator that iterates through the parameters in the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this._parameters.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the parameters in the collection.
        /// </summary>
        /// <returns>
        /// Returns an enumerator that iterates through the parameters in the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._parameters.GetEnumerator();
        }
    }

}
