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
    internal static class Document
    {
        /// <summary>
        /// Async method, return Response xmlDocument
        /// </summary>
        /// <param name="xmlRequest">xml request document</param>
        /// <returns></returns>
        public static async Task<XmlDocument> SendRequestXmlDocumentAsync(XmlDocument xmlRequest)
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.novaposhta.ua/v2.0/xml/");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = @"application/x-www-form-urlencoded";
            ServicePointManager.DefaultConnectionLimit = 2000;

            var streamOut = new StreamWriter(await httpWebRequest.GetRequestStreamAsync());
            await streamOut.WriteAsync(xmlRequest.InnerXml);

            //streamOut.Flush();
            streamOut.Close();

            //In Stream
            var response = (await httpWebRequest.GetResponseAsync()).GetResponseStream();

            if (response == null)
                return null;

            var streamIn = new StreamReader(response);

            string strResponse = await streamIn.ReadToEndAsync();
            streamIn.Close();
            response.Close();

            //Load XML data to XmlDocument
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(strResponse);

            return xmlResponse;
        }

        /// <summary>
        /// return Response xmlDocument
        /// </summary>
        /// <param name="xmlRequest">xml request document</param>
        /// <returns></returns>
        public static XmlDocument SendRequestXmlDocument(XmlDocument xmlRequest)
        {
            //HttpWebRequest to a Web Service
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.novaposhta.ua/v2.0/xml/");

            //Properties of connection
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = @"application/x-www-form-urlencoded";
            //  httpWebRequest.Timeout = 12000;
            ServicePointManager.DefaultConnectionLimit = 1000;

            //Out stream
            var streamOut = new StreamWriter(httpWebRequest.GetRequestStream());

            streamOut.Write(xmlRequest.InnerXml);

            // streamOut.Flush();
            streamOut.Close();

            //In Stream
            var response = httpWebRequest.GetResponse().GetResponseStream();

            if (response == null)
                return null;

            var streamIn = new StreamReader(response);

            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            response.Close();

            //Load XML data to XmlDocument
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(strResponse);

            return xmlResponse;
        }

        /// <summary>
        /// Return xmlDocument with query to Web API
        /// </summary>
        /// <param name="APIkey">API key of NovaPoshta (can get in your account in NovaPoshta)</param>
        /// <param name="modelName">modelName</param>
        /// <param name="methodName">Name of calling method in Web API</param>
        /// <param name="methodPropetries">Parameters of calling method</param>
        /// <returns></returns>
        public static XmlDocument MakeRequestXmlDocument(string APIkey, string modelName, string methodName, XmlNodeList methodPropetries)
        {
            string query = @"<?xml version='1.0' encoding='UTF-8'?><root><apiKey></apiKey><modelName></modelName>
<calledMethod></calledMethod><methodProperties></methodProperties></root>";

            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(query);

            foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "apiKey":
                        node.InnerText = APIkey;
                        break;
                    case "modelName":
                        node.InnerText = modelName;
                        break;
                    case "calledMethod":
                        node.InnerText = methodName;
                        break;
                    case "methodProperties":
                        if (methodPropetries == null) break;
                        foreach (XmlNode propetry in methodPropetries)
                        {
                            node.AppendChild(xmlDocument.ImportNode(propetry, true));
                        }

                        break;
                }
            }


            return xmlDocument;
        }
    }
    internal class Document<T> where T : IComponent, new()
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
