using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PostWatcher;

namespace API_NovaPoshta
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("Cyrillic");



            XmlDocument xmlRequest = new XmlDocument();


            xmlRequest = API_NovaPoshta._makeXmlDocument("89d098a380862faab14d9196653823dc", "InternetDocument",
               "getDocumentList", new Dictionary<string, string>() { { "DateTime", "" } });

            API_NovaPoshta._XmlReader(API_NovaPoshta._Request(xmlRequest));

            Console.ReadLine();
        }

      
      
        

    }

   
}
