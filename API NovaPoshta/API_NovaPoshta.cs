using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace API_NovaPoshta
{
    static class API_NovaPoshta
    {
        /// <summary>
        /// Make a request to web service
        /// </summary>
        /// <param name="xmlRequest">XML file with query</param>
        /// <returns>XML document with result of query</returns>
        public static XmlDocument _Request(XmlDocument xmlRequest)
        {
            //HttpWebRequest to a Web Service
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.novaposhta.ua/v2.0/xml/");

            //Properties of connection
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = @"application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = xmlRequest.InnerXml.Length;

            //Out stream
            var streamOut = new StreamWriter(httpWebRequest.GetRequestStream());
            streamOut.Write(xmlRequest.InnerXml);
            streamOut.Close();

            //In Stream
            var response = httpWebRequest.GetResponse().GetResponseStream();

            if (response == null)
                return null;

            var streamIn = new StreamReader(response);
            //Read Infomation
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            response.Close();

            //Load XML data to XmlDocument
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(strResponse);

            //Write received data to file
            //var file = new FileStream("Output.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            //var streamWriter = new XmlTextWriter(file, Encoding.UTF8);
            //xmlResponse.Save(streamWriter);

            //streamWriter.Close();
            //file.Close();

            return xmlResponse;
        }


        public static void _XmlReader(XmlDocument xmlDocument)
        {
            Console.WriteLine(xmlDocument.DocumentElement.Name);

            var query = from XmlNode x in xmlDocument.DocumentElement.ChildNodes
                        select x;


            //foreach (var item in query)
            //{
            //    Console.WriteLine(item.Name);
            //    foreach (XmlNode nodes in item.ChildNodes)
            //    {
            //        Console.WriteLine(nodes.Name + " " + nodes.Value);
            //    }
            //}


            XmlNode xmlNode = query.Single((XmlNode x) => x.Name == "data");

            var getItems = from XmlNode x in xmlNode.ChildNodes
                           select x;

            //  Console.WriteLine(xmlNode.Name);
            foreach (var item in getItems)
            {
                var getItemInfo = from XmlNode x in item.ChildNodes
                                  select x;

                Console.WriteLine(item.Name);

                foreach (var info in getItemInfo)
                {
                    Console.WriteLine("\t" + info.Name + "\t" + info.InnerText);
                }


            }

            Console.ReadLine();
        }

        /// <summary>
        /// Make XML document with query
        /// </summary>
        /// <param name="APIkey">api key</param>
        /// <param name="modelName">model Name</param>
        /// <param name="methodName">CalledMethod nane</param>
        /// <param name="methodPropetries">propetries of method query</param>
        /// <returns>XML document with query</returns>
        public static XmlDocument _makeXmlDocument(string APIkey, string modelName, string methodName, Dictionary<string, string> methodPropetries)
        {
            string query = @"<?xml version=""1.0"" encoding=""UTF-8""?><root><apiKey></apiKey><modelName></modelName>
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
                        for (int i = 0; i < methodPropetries.Count; i++)
                        {
                            var newNode = xmlDocument.CreateNode(XmlNodeType.Element, methodPropetries.Keys.ElementAt(i), null);
                            newNode.InnerText = methodPropetries.Values.ElementAt(i);

                            node.AppendChild(newNode);
                        }
                        break;
                }
            }


            return xmlDocument;
        }


        public static XmlDocument _makeXmlDocument(string APIkey, string modelName, string methodName, XmlNode methodPropetries)
        {
            string query = @"<?xml version=""1.0"" encoding=""UTF-8""?><root><apiKey></apiKey><modelName></modelName>
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
                        node.AppendChild(methodPropetries);
                        break;
                }
            }


            return xmlDocument;
        }

    }
}
