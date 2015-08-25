using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace PostWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        //USE DATA BASEE!!!!!!!!!!!!!


        private string _APIKey;
        private DataItem filter = new DataItem();
        private List<Document> responseDocuments = new List<Document>(7);
        private DateBaseOfDocuments dbDocs;
        private List<string> stateFilter = new List<string>(10);
        private readonly IsolatedStorageFile _isolated = IsolatedStorageFile.GetUserStoreForAssembly();
        private object locker = new Object();
        private Task findTask;
        private CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();

            DatePickerLeft.DisplayDateEnd = DateTime.Today;
            DatePickerLeft.SelectedDate = DateTime.Today;
            DatePickerRight.DisplayDateEnd = DateTime.Today;
            DatePickerRight.SelectedDate = DateTime.Today;
        }
        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var rk = Registry.CurrentUser.OpenSubKey("PostWatcher");
            bool isLogined = true;

            if (rk == null)
            {
                rk = Registry.CurrentUser.CreateSubKey("PostWatcher");
                isLogined = false;
            }
            else
                if (rk.GetValue("API key") == null)
                    isLogined = false;


            if (!isLogined)
            {
                var newWindwowApIkey = new APIkey();
                newWindwowApIkey.ShowDialog();
                _APIKey = (string)rk.GetValue("API key");

                OpenLoader("RefreshDataBase");
            }

            _APIKey = (string)rk.GetValue("API key");

            dbDocs = await GetFiles();

            await Task.Factory.StartNew(
                () => AddItemsToDataGrid(dbDocs.Documents, filter)
                                       );

            if (dbDocs.Dates.Max < DateTime.Today)
                OpenLoader("RefreshDateBase");

        }

        private void OpenLoader(string methodName)
        {
            var newLoading = new Loading(_APIKey, methodName);
            newLoading.ShowDialog();
        }

        #region MENU EVENT HANDLER METHODS

        private void menuChangeAPIkey_OnClick(object sender, RoutedEventArgs e)
        {
            var newWindow = new APIkey();
            newWindow.ShowDialog();
            var rk = Registry.CurrentUser.OpenSubKey("PostWatcher");
            _APIKey = (string)rk.GetValue("API key");

        }

        private void menuExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuResresh_OnClick(object sender, RoutedEventArgs e)
        {
            Btn_OKfilter_OnClick(sender, e);
        }

        #endregion

        private void AsyncChangeControlState(Control element, Action action)
        {
            if (element.Dispatcher.CheckAccess())
                action();
            else
                prb_state.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    action);
        }

        protected async Task<bool> CheckConnectionAsync()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = await client.OpenReadTaskAsync("http://www.google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<DateBaseOfDocuments> GetFiles()
        {
            string[] fileNames;
            string fileName = String.Empty;
            lock (locker)
            {
                fileNames = _isolated.GetFileNames();

                if (fileNames.Length == 0)
                    return null;

                fileName = fileNames.Single((fileN) => fileN == _APIKey);
            }

            return await DeSerializeDocuments(fileName);
        }

        private void AddItemsToDataGrid(XmlDocument xmlDocument, DataItem filterItem)
        {
            var document = new Document();
            document.LoadResponseXmlDocument(xmlDocument);

            if (!document.Success || !document.HasData)
                return;

            foreach (var dataItem in document.Items)
            {
                if (!CompareTwoItems(dataItem, filterItem)) continue;
                if (stateFilter.Contains(dataItem.StateName)) continue;

                var thisValue = dataItem;
                AsyncChangeControlState(DG_doc, () => DG_doc.Items.Add(thisValue));
            }
        }
    
        private void AddItemsToDataGrid(List<Document> docs, DataItem filterItem)
        {
            foreach (var doc in docs)
            {
                AddItemsToDataGrid(doc, filterItem);
            }
        }

        private void AddItemsToDataGrid(Document document, DataItem filterItem)
        {
            if (!document.Success || !document.HasData)
                return;

            AddItemsToDataGrid(document.Items, filterItem);
        }

        private void AddItemsToDataGrid(List<DataItem> items, DataItem filterItem)
        {
            foreach (var item in items)
            {
                AddItemsToDataGrid(item, filterItem);
            }
        }

        private void AddItemsToDataGrid(DataItem item, DataItem filterItem)
        {
            if (!CompareTwoItems(item, filterItem)) return;
            if (stateFilter.Contains(item.StateName)) return;

            var thisValue = item;
            AsyncChangeControlState(DG_doc, () => DG_doc.Items.Add(thisValue));
        }

        private static bool CompareTwoItems (DataItem self, DataItem to)
        {
            if (self == null) return false;
            if (to == null) return true;

            var type = self.GetType();
            var countOfEquals = 0;
            var query = new List<object>();

            foreach (
                var pi in type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
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
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

        #region BUTTONS

        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private async void Btn_OKfilter_OnClick(object sender, RoutedEventArgs e)
        {
            var items = DG_doc.Items.Cast<DataItem>().ToList();
            DG_doc.Items.Clear();
            btn_OKfilter.IsEnabled = false;
            await Task.Factory.StartNew(() => AddItemsToDataGrid(items, filter));
            btn_OKfilter.IsEnabled = true;
        }

        private async void Btn_selectDataOK_OnClick(object sender, RoutedEventArgs e)
        {
            DG_doc.Items.Clear();

            DateTime left = DatePickerLeft.SelectedDate ?? DateTime.Today;
            DateTime right = DatePickerRight.SelectedDate ?? DateTime.Today;

            await Task.Factory.StartNew(() =>
            {
                var query = from x in dbDocs.Documents
                    where (x.Date >= left) && (x.Date <= right)
                    select x;

                AddItemsToDataGrid(query.ToList(), filter);
            });
        }

        #endregion

        #region TEXTBOX ADD TO FILTER

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
        #endregion

        #region COMBO_BOX OF STATE FILTER
        private void Chb_delievered_OnChecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Remove("Одержаний");
        }

        private void Chb_Proccessing_OnChecked(object sender, RoutedEventArgs e)
        {

            stateFilter.Remove("Замовлення в обробці");
        }

        private void Chb_WaitingForSending_OnChecked(object sender, RoutedEventArgs e)
        {

            stateFilter.Remove("Готується до відправлення");
        }

        private void Chb_ArrivedToRecipient_OnChecked(object sender, RoutedEventArgs e)
        {

            stateFilter.Remove("Прибув у відділення");
        }

        private void Chb_Sended_OnChecked(object sender, RoutedEventArgs e)
        {

            stateFilter.Remove("Відправленно");
        }

        private void Chb_AddressChanged_OnChecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Remove("Змінено адресу");
        }

        private void Chb_Sended_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Відправленно");
        }

        private void Chb_AddressChanged_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Змінено адресу");
        }

        private void Chb_ArrivedToRecipient_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Прибув у відділення");
        }

        private void Chb_WaitingForSending_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Готується до відправлення");
        }

        private void Chb_Proccessing_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Замовлення в обробці");
        }

        private void Chb_delievered_OnUnchecked(object sender, RoutedEventArgs e)
        {
            stateFilter.Add("Одержаний");
        }
        #endregion

        #region DataGrid events


        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = DG_doc.SelectedItems.Cast<DataItem>().ToList();
            var dataGridItems = DG_doc.Items;



            var newXmlDocument = await Task<List<DataItem>>.Factory.StartNew(() =>
            {
                var methodPrepetries = CreateXmlListPropertiesForDocumentsTracking(selectedItems);

                var task = MakeTask("InternetDocument", "documentsTracking", methodPrepetries);

                var document = new Document();
                document.LoadResponseXmlDocument(task.Result);

                return document.Items;
            });

            for (var i = 0; i < selectedItems.Count; i++)
            {
                var index = dataGridItems.IndexOf(selectedItems[i]);
                selectedItems[i].StateName = newXmlDocument[i].StateName;
                //  var oldResponseList = await DeSerializeDocuments();
                dataGridItems[index] = selectedItems[i];
            }
        }


        #endregion

        private async Task<DateBaseOfDocuments> DeSerializeDocuments(string fileName)
        {
            var stream = _isolated.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Write);
            var binnaryFormatter = new BinaryFormatter();

            var responseList = binnaryFormatter.Deserialize(stream) as DateBaseOfDocuments;
            return responseList;
        }

        private XmlNodeList CreateXmlListPropertiesForGetDocuments(DateTime left, int x)
        {
            var current = left.AddDays(x);
            var xmlDoc = new XmlDocument();
            var methodPropetriesNode = xmlDoc.CreateNode(XmlNodeType.Element, "DateTime", null);
            methodPropetriesNode.InnerText = String.Format("{0}.{1}.{2}", current.Day, current.Month, current.Year);
            xmlDoc.AppendChild(methodPropetriesNode);
            XmlNodeList xmlList = xmlDoc.ChildNodes;
            return xmlList;
        }

        private XmlNodeList CreateXmlListPropertiesForDocumentsTracking(List<DataItem> selectItems)
        {
            var xmlDoc = new XmlDocument();
            var methodPropetriesNode = xmlDoc.CreateNode(XmlNodeType.Element, "Documents", null);
            foreach (var item in selectItems)
            {
                var newItem = xmlDoc.CreateNode(XmlNodeType.Element, "item", null);
                newItem.InnerText = item.IntDocNumber;
                methodPropetriesNode.AppendChild(newItem);
            }
            xmlDoc.AppendChild(methodPropetriesNode);
            XmlNodeList xmlList = xmlDoc.ChildNodes;
            return xmlList;
        }



    }
}

