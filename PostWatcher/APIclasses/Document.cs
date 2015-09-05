using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    public class Document<T> where T : IComponent, new()
    {

        private bool _success;
        private bool _hasData;
        private string _error;
        private List<T> _items = new List<T>();

      /// <summary>
        /// List of items in this document
        /// </summary>  
        public List<T> Items
        {
            get
            {
                if (!_hasData)
                    return null;
                
                return _items;
            }
          private set { _items = value; }
        }

        /// <summary>
        /// if Query to Web API is success
        /// </summary>
        public bool Success
        {
            get { return _success; }
            private set { _success = value; }
        }

        /// <summary>
        /// if Document has items
        /// </summary>
         public bool HasData
        {
            get { return _hasData; }
            private set { _hasData = value; }
        }

        /// <summary>
        /// Error message 
        /// </summary>
         public string Error
        {
            get { return _error; }
            private set { _error = value; }
        }

      
        /// <summary>
        /// Initialize properties of this instanse
        /// </summary>
        /// <param name="xmlDoc">Response xmlDocument</param>
        public void LoadFromXml(XmlDocument xmlDoc)
        {

            var query = from XmlNode x in xmlDoc.DocumentElement.ChildNodes
                        select x;
            
            foreach (var root in query)
            {
                switch (root.Name)
                {
                    case "success":
                        Boolean.TryParse(root.InnerText, out _success);
                        break;
                    case "data":
                        _hasData = root.HasChildNodes;
                        break;
                    case "errors":
                        if (!_success)
                        {
                            var errorQuery = from XmlNode x in root.ChildNodes
                                             where x.Name == "item"
                                             select x.InnerText;

                            var strb = new StringBuilder();

                            foreach (string error in errorQuery)
                            {
                                strb.Append(error + "\n");
                            }

                            _error = strb.ToString();
                        }
                        break;
                }
            }

            if (!_hasData)
                return;

            var dataNode = xmlDoc.DocumentElement.SelectSingleNode("data");

            foreach (XmlNode item in dataNode.ChildNodes)
            {
                T container = new T();
                container.LoadFromXml(item);
                _items.Add(container);
            }

        }
    }
}
