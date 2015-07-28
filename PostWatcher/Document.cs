using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    class Document
    {
        private bool _success = false;
        private bool _hasData = false;
        private string _error;
        private List<DataItem> _items = new List<DataItem>();

        public List<DataItem> Items
        {
            get
            {
                if (_hasData)
                    return _items;
                else return null;
            }
            private set { _items = value; }
        }

        public bool Success
        {
            get { return _success; }
            private set { _success = value; }
        }

        public bool HasData
        {
            get { return _hasData; }
            private set { _hasData = value; }
        }

        public string Error
        {
            get { return _error; }
        }
      
        public  XmlDocument SendRequestXmlDocument(XmlDocument xmlRequest)
        {
            //HttpWebRequest to a Web Service
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.novaposhta.ua/v2.0/xml/");

            //Properties of connection
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = @"application/x-www-form-urlencoded";
            httpWebRequest.Timeout = 4000;
            ServicePointManager.DefaultConnectionLimit = 1000;

            //Out stream
            var streamOut = new StreamWriter(httpWebRequest.GetRequestStream());

            streamOut.Write(xmlRequest.InnerXml);

            streamOut.Flush();
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

        public XmlDocument MakeRequestXmlDocument(string APIkey, string modelName, string methodName, XmlNodeList methodPropetries)
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

        public void LoadResposneXmlDocument(XmlDocument xmlDoc)
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
                var dateItem = new DataItem();
                dateItem.LoadXml(item);
                _items.Add(dateItem);
            }

        }

    }
}
