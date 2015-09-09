using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PostWatcher
{
    public class APImethods
    {
        private static string _apiKey;

        public APImethods(string apiKey)
        {
            _apiKey = apiKey;
        }

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
        public static async Task<XmlDocument> SendRequestXmlDocument(XmlDocument xmlRequest)
        {
            //HttpWebRequest to a Web Service
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.novaposhta.ua/v2.0/xml/");

            //Properties of connection
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = @"application/x-www-form-urlencoded";
            //  httpWebRequest.Timeout = 12000;
            ServicePointManager.DefaultConnectionLimit = 1000;

            //Out stream
            var streamOut = new StreamWriter(await httpWebRequest.GetRequestStreamAsync());

            await streamOut.WriteAsync(xmlRequest.InnerXml);

            // streamOut.Flush();
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


        public async Task<Document<City>> GetCitiesAsync(XmlNodeList methodProperties)
        {
            var xmlResponse = await MakeTask("Address", "getCities", methodProperties);
            var doc = new Document<City>();
            doc.LoadFromXml(xmlResponse);
            return doc;
        }
        private Document<City> GetCities(XmlNodeList methodProperties)
        {
            var xmlResponse = MakeTask("Address", "getCities", methodProperties);
            var doc = new Document<City>();
            doc.LoadFromXml(xmlResponse.Result);
            return doc;
        }


        public async Task<Document<TrackingItem>> DocumentsTrackingAsync(XmlNodeList methodProperties)
        {
            var task = await MakeTask("InternetDocument", "documentsTracking", methodProperties);

            var document = new Document<TrackingItem>();
            document.LoadFromXml(task);
            return document;
        } 
        public Document<TrackingItem> DocumentsTracking(XmlNodeList methodProperties)
        {
            var task = MakeTask("InternetDocument", "documentsTracking", methodProperties);

            var document = new Document<TrackingItem>();
            document.LoadFromXml(task.Result);
            return document;
        }

        public async Task<Document<DataItem>> GetDocumentListAsync(XmlNodeList methodProperties)
        {
            var xmlResponse = await MakeTask("InternetDocument", "getDocumentList", methodProperties);
            var doc = new Document<DataItem>();
            doc.LoadFromXml(xmlResponse);
            return doc;
        }
        public Document<DataItem> GetDocumentList(XmlNodeList methodProperties)
        {
            var xmlResponse = MakeTask("InternetDocument", "getDocumentList", methodProperties);
            var doc = new Document<DataItem>();
            doc.LoadFromXml(xmlResponse.Result);
            return doc;
        }

        private async Task<XmlDocument> MakeTask(string modelName, string methodName, XmlNodeList xmlList)
        {

            var xmlQuery = MakeRequestXmlDocument(_apiKey, modelName, methodName, xmlList);

            XmlDocument xmlResponse = null;
            try
            {
                Thread.Sleep(new Random().Next(50));
                xmlResponse = await SendRequestXmlDocumentAsync(xmlQuery);
            }
            catch (WebException e)
            {
                throw e;
            }

            return xmlResponse;
        }




    }
}
