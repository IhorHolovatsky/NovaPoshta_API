using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
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
        private static DataItem filter = new DataItem();
        private static List<Action> actions = new List<Action>();
        private static Thread _newThread;
        private readonly IsolatedStorageFile _isolated = IsolatedStorageFile.GetUserStoreForAssembly();

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

        private void AsyncChangeControlState(Control element, bool state)
        {
            if (element.Dispatcher.CheckAccess())
                element.IsEnabled = state;
            else
                element.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => element.IsEnabled = state));
        }

        private void AsyncChangeControlVisibility(Control element, Visibility state)
        {
            if (element.Dispatcher.CheckAccess())
                element.Visibility = state;
            else
                prb_state.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                    () => element.Visibility = state

                    )
                    );
        }



        private void Btn_selectDataOK_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckConnection())
            {
                MessageBox.Show("No Internet Connection!");
                return;
            }

            if (_newThread != null)
                if (_newThread.IsAlive)
                    return;

            DateTime left = DatePickerLeft.SelectedDate ?? DateTime.Today;
            DateTime right = DatePickerRight.SelectedDate ?? DateTime.Today;
            prb_state.Value = 0;


            _newThread = new Thread(
                () => _GetNovaPoshtaDocuments(left, right)
                                  );
            _newThread.Start();


        }

        private void _GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
            foreach (var file in _isolated.GetFileNames())
            {
                _isolated.DeleteFile(file);
            }
            AsyncChangeControlVisibility(prb_state, Visibility.Visible);
            GetNovaPoshtaDocuments(left, right);
            AsyncChangeControlVisibility(prb_state, Visibility.Hidden);
        }

        private void GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
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

            for (int i = 0; i < (right - left).Days + 1; i++)
            {
                if (prb_state.Dispatcher.CheckAccess())
                    prb_state.Value += 1000.0 / ((right - left).Days + 1);
                else
                    prb_state.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                        () => prb_state.Value += 1000.0 / ((right - left).Days + 1)
                        )
                        );

                var current = left.AddDays(i);

                var xmlDoc = new XmlDocument();
                var methodPropetriesNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateTime",
                    null);
                methodPropetriesNode.InnerText = String.Format("{0}.{1}.{2}", current.Day, current.Month, current.Year);
                xmlDoc.AppendChild(methodPropetriesNode);
                XmlNodeList xmlList = xmlDoc.ChildNodes;

                var newDocument = new Document();
                var xmlQuery = newDocument.MakeRequestXmlDocument(_APIKey, _modelName, _methodName, xmlList);

                XmlDocument xmlResponse = null;
                try
                {
                    xmlResponse = newDocument.SendRequestXmlDocument(xmlQuery);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);
                    AsyncChangeControlVisibility(prb_state, Visibility.Hidden);
                    Thread.CurrentThread.Abort();
                }
                SaveRequest(xmlResponse, i.ToString());

                AddItemsToDataGrid(xmlResponse, filter);
            }
        }

        protected bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveRequest(XmlDocument xmlDocument, string name)
        {

            // var isolatedStream = new IsolatedStorageFileStream("Request" + name +".xml", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            var writer = new XmlTextWriter(_isolated.CreateFile("Request" + name + ".xml"), Encoding.UTF8);
            xmlDocument.Save(writer);
            writer.Close();

        }

        private void GetFiles()
        {
            DG_doc.Items.Clear();

            foreach (var xmlDoc in _isolated.GetFileNames().Select(ReadFile))
            {
                AddItemsToDataGrid(xmlDoc, filter);
            }
        }

        private XmlDocument ReadFile(string file)
        {
            var fileStream = new XmlTextReader(_isolated.OpenFile(file, FileMode.Open, FileAccess.Read, FileShare.Write));
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileStream);
            fileStream.Close();
            return xmlDoc;
        }


        private void AddItemsToDataGrid(XmlDocument xmlDocument, DataItem filterDocument)
        {
            var document = new Document();
            document.LoadResposneXmlDocument(xmlDocument);


            if (!document.Success)
                MessageBox.Show(document.Error);

            if (!document.HasData)
                return;

            foreach (var dataItem in document.Items)
            {
                if (!CompareTwoDocuments(dataItem, filterDocument)) continue;

                if (DG_doc.Dispatcher.CheckAccess())
                {

                    DG_doc.Items.Add(dataItem);
                }
                else
                {
                    DataItem thisValue = dataItem;
                    DG_doc.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() => DG_doc.Items.Add(thisValue)));
                }
            }

        }

        private static bool CompareTwoDocuments(DataItem self, DataItem to)
        {
            if (self == null) return false;
            if (to == null) return true;

            var type = self.GetType();
            //var query = from pi in type.GetProperties(BindingFlags.Public)
            //    let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
            //    let toValue = type.GetProperty(pi.Name).GetValue(to, null)
            //    where (selfValue == toValue)
            //    select selfValue;
            int countOfEquals = 0;
            List<object> query = new List<object>();

            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (selfValue == null || toValue == null) continue;


                var isDefault = toValue.Equals(GetDefault(pi.PropertyType));

                if (!isDefault)
                    countOfEquals++;

                if (selfValue.Equals(toValue) && !isDefault)
                {
                    query.Add(selfValue);
                }
            }

            return query.Count() == countOfEquals;

        }

        private static object GetDefault(Type t)
        {
            //if (t == typeof(DateTime))
            //    return DateTime.MinValue;
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }



        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (_newThread.IsAlive)
                _newThread.Abort();
            prb_state.Visibility = Visibility.Hidden;
        }

        private void Cb_StateName_OnSelected(object sender, RoutedEventArgs e)
        {
            var innerText = ((TextBlock)cb_StateName.SelectedItem).Text;
            filter.StateName = innerText == "" ? null : innerText;
        }


        private void Btn_OKfilter_OnClick(object sender, RoutedEventArgs e)
        {
            GetFiles();
        }

        private void Tb_IntDoc_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            filter.IntDocNumber = tb_IntDoc.Text != "" ? tb_IntDoc.Text : null;
        }

        private void Tb_RecipientCityDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            filter.CityRecipientDescription = tb_RecipientCityDescription.Text != "" ? 
                tb_RecipientCityDescription.Text : null;
        }

        private void Tb_RecipientPhone_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            filter.RecipientContactPhone = tb_RecipientPhone.Text != "" ? tb_RecipientPhone.Text : null;
        }
    }
}

