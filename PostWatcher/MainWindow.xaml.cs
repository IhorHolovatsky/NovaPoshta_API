using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using API_NovaPoshta = API_NovaPoshta.API_NovaPoshta;

namespace PostWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _APIKey = "fd00953407f9e0ac0c86a94cdc91c33c";
        private static string _modelName;
        private static string _methodName;
        private static Dictionary<string, string> _methodPropetries = new Dictionary<string, string>();
        private static Thread newThread;

        private IsolatedStorageFile isolated = IsolatedStorageFile.GetUserStoreForAssembly();



        int count = 0;
        double NPcost = 0;
        double MyCost = 0;


        public MainWindow()
        {
            InitializeComponent();

            DatePickerLeft.DisplayDateEnd = DateTime.Today;
            DatePickerLeft.SelectedDate = DateTime.Today;
            DatePickerRight.DisplayDateEnd = DateTime.Today;
            DatePickerRight.SelectedDate = DateTime.Today;

            GetFiles();
        }

        //Creating a function that uses the API function...

        private void menuChangeAPIkey_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void menuExit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void menuResresh_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ChangeControlState(Control element, bool state)
        {
            if (element.Dispatcher.CheckAccess())
                element.IsEnabled = state;
            else
                element.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => element.IsEnabled = state));
        }


        private void ChangeControlVisibility(Control element, Visibility state)
        {
            if (element.Dispatcher.CheckAccess())
                element.Visibility =state;
            else
                prb_state.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                    () => element.Visibility = state
                        
                    )
                    );
        }



        private void Btn_selectDataOK_OnClick(object sender, RoutedEventArgs e)
        {

            DateTime left = DatePickerLeft.SelectedDate ?? DateTime.Today;
            DateTime right = DatePickerRight.SelectedDate ?? DateTime.Today;
          
            if (!CheckConnection())
            {
                MessageBox.Show("No Internet Connection!");
                return;
            }

            ChangeControlState(btn_selectDataOK, false);
            prb_state.Value = 0;

            newThread = new Thread(
                () => _GetNovaPoshtaDocuments(left, right)
                                  );

            newThread.Start();


        }

        private void _GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
           
           
                foreach (var file in isolated.GetFileNames())
                {
                    isolated.DeleteFile(file);
                }
           
            
            ChangeControlVisibility(prb_state, Visibility.Visible);

            GetNovaPoshtaDocuments(left, right);

            ChangeControlState(btn_selectDataOK, true);

            ChangeControlVisibility(prb_state, Visibility.Hidden);

        }

        private void GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
            ChangeControlState(prb_state, true);

            if (DG_doc.Dispatcher.CheckAccess())
            {
                DG_doc.Items.Clear();
            }
            else
            {
                DG_doc.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => DG_doc.Items.Clear()));
            }

            _modelName = "InternetDocument";
            _methodName = "getDocumentList";

            var current = left;

            for (int i = 0; i < (right - left).Days + 1; i++)
            {
                if (prb_state.Dispatcher.CheckAccess())
                    prb_state.Value += 1000.0/((right - left).Days + 1);
                else
                    prb_state.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                        () => prb_state.Value += 1000.0/((right - left).Days + 1)
                        )
                        );

                var temp = current.AddDays(i);

                _methodPropetries.Add("DateTime", String.Format("{0}.{1}.{2}", temp.Day, temp.Month, temp.Year));
                var xmlQuery = MakeXmlQuery(_APIKey, _modelName, _methodName, _methodPropetries);
                XmlDocument xmlResponse = null;
                try
                {
                    xmlResponse = SendRequest(xmlQuery);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                    Thread.CurrentThread.Abort();
                }

                SaveRequest(xmlResponse, i.ToString());
                ReadXml(xmlResponse);
                _methodPropetries.Clear();
            }
        }

        protected bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com")) ;



                return true;
            }
            catch
            {
                return false;
            }


        }

        private XmlDocument MakeXmlQuery(string APIkey, string modelName, string methodName,
            Dictionary<string, string> methodPropetries)
        {
            return global::API_NovaPoshta.API_NovaPoshta._makeXmlDocument(APIkey, modelName, methodName, methodPropetries);
        }
        private XmlDocument SendRequest(XmlDocument xmlDocument)
        {
            return global::API_NovaPoshta.API_NovaPoshta._Request(xmlDocument);
        }

        private void SaveRequest(XmlDocument xmlDocument, string name)
        {

            // var isolatedStream = new IsolatedStorageFileStream("Request" + name +".xml", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            var writer = new XmlTextWriter(isolated.CreateFile("Request" + name + ".xml") as FileStream, Encoding.UTF8);
            xmlDocument.Save(writer);
            writer.Close();

        }


        private void GetFiles()
        {
            foreach (var file in isolated.GetFileNames())
            {
                var xmlDoc = ReadFile(file);
                ReadXml(xmlDoc);
            }

        }

        private XmlDocument ReadFile(string file)
        {
            var fileStream = new XmlTextReader(isolated.OpenFile(file, FileMode.Open, FileAccess.Read, FileShare.Write));
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileStream);
            fileStream.Close();
            return xmlDoc;
        }


        private void ReadXml(XmlDocument xmlDocument)
        {

            var Document = new Document();
            Document.LoadResposneXml(xmlDocument);

            if (!Document.Success)
                MessageBox.Show(Document.Error);

            if (!Document.HasData)
                return;

            if (DG_doc.Dispatcher.CheckAccess())
            {

                DG_doc.Items.Add(Document.Items);
            }
            else
            {
                DG_doc.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => DG_doc.Items.Add(Document.Items)));
            }
        }

        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (newThread.IsAlive)
                newThread.Abort();
            btn_selectDataOK.IsEnabled = true;
        }
    }
}

