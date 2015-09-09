using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using Microsoft.Win32;

namespace PostWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private string _apiKey;
        private readonly DataItem _filter = new DataItem();
        private string _connectionString;
        private readonly List<string> _stateFilter = new List<string>(10);

        public MainWindow()
        {
            InitializeComponent();

            DatePickerLeft.DisplayDateEnd = DateTime.Today;
            DatePickerLeft.SelectedDate = DateTime.Today;
            DatePickerRight.DisplayDateEnd = DateTime.Today;
            DatePickerRight.SelectedDate = DateTime.Today;
        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectToTTN"].ConnectionString;
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
                _apiKey = (string)rk.GetValue("API key");

                OpenLoader("InternetDocument", "getDocumentList", null);
            }

            _apiKey = (string)rk.GetValue("API key");

            //Bad realization
            //Refresh all Libraries of NovaPoshta.
            OpenLoader("InternetDocument", "getDocumentList", null);
        }

        private void OpenLoader(string modelName, string methodName, XmlNodeList methodProperties)
        {
            var newLoading = new Loading(_apiKey, modelName, methodName, methodProperties);
            newLoading.ShowDialog();
        }

        #region MENU EVENT HANDLER METHODS

        private void menuChangeAPIkey_OnClick(object sender, RoutedEventArgs e)
        {
            var newWindow = new APIkey();
            newWindow.ShowDialog();
            var rk = Registry.CurrentUser.OpenSubKey("PostWatcher");
            _apiKey = (string)rk.GetValue("API key");

        }

        private void menuExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuResresh_OnClick(object sender, RoutedEventArgs e)
        {

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
                using (await client.OpenReadTaskAsync("http://www.google.com")) return true;
            }
            catch
            {
                return false;
            }
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
            if (_stateFilter.Contains(item.StateName)) return;

            var thisValue = item;
            AsyncChangeControlState(DG_doc, () => DG_doc.Items.Add(thisValue));
        }

        private static bool CompareTwoItems(DataItem self, DataItem to)
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


        private async void Btn_selectDataOK_OnClick(object sender, RoutedEventArgs e)
        {
            DG_doc.Items.Clear();

            DateTime left = DatePickerLeft.SelectedDate ?? DateTime.Today;
            DateTime right = DatePickerRight.SelectedDate ?? DateTime.Today;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (
                SqlCommand cmd = new SqlCommand("SELECT * FROM [TTN] WHERE DateTime BETWEEN @left AND @right ", connection))
            {
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("@left", left);
                cmd.Parameters.AddWithValue("@right", right);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            var dataItem = new DataItem();

                            dataItem.IntDocNumber = reader.GetString(0).Trim();
                            dataItem.DateTime = reader.GetDateTime(1);
                            dataItem.CityRecipientDescription = reader.GetString(2).Trim();
                            dataItem.RecipientDescription = reader.GetString(3).Trim();
                            dataItem.RecipientAddressDescription = reader.GetString(4).Trim();
                            dataItem.RecipientContactPhone = reader.GetString(5).Trim();
                            dataItem.Weight = reader.GetDouble(6);
                            dataItem.Cost = reader.GetDouble(7);
                            dataItem.CostOnSite = reader.GetDouble(8);
                            dataItem.StateName = reader.GetString(9).Trim();
                            dataItem.PrintedDescription = reader.GetString(10).Trim();


                            AddItemsToDataGrid(dataItem, _filter);
                        }
                }
            }

        }

        #endregion

        #region TEXTBOX ADD TO FILTER

        private void Tb_IntDoc_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _filter.IntDocNumber = tb_IntDoc.Text != "" ? tb_IntDoc.Text : null;
        }

        private void Tb_RecipientCityDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _filter.CityRecipientDescription = tb_RecipientCityDescription.Text != "" ?
                tb_RecipientCityDescription.Text : null;
        }

        private void Tb_RecipientPhone_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _filter.RecipientContactPhone = tb_RecipientPhone.Text != "" ? tb_RecipientPhone.Text : null;
        }
        #endregion

        #region COMBO_BOX OF STATE FILTER
        private void Chb_delievered_OnChecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Remove("Одержаний");
        }

        private void Chb_Proccessing_OnChecked(object sender, RoutedEventArgs e)
        {

            _stateFilter.Remove("Замовлення в обробці");
        }

        private void Chb_WaitingForSending_OnChecked(object sender, RoutedEventArgs e)
        {

            _stateFilter.Remove("Готується до відправлення");
        }

        private void Chb_ArrivedToRecipient_OnChecked(object sender, RoutedEventArgs e)
        {

            _stateFilter.Remove("Прибув у відділення");
        }

        private void Chb_Sended_OnChecked(object sender, RoutedEventArgs e)
        {

            _stateFilter.Remove("Відправленно");
        }

        private void Chb_AddressChanged_OnChecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Remove("Змінено адресу");
        }

        private void Chb_Sended_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Відправленно");
        }

        private void Chb_AddressChanged_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Змінено адресу");
        }

        private void Chb_ArrivedToRecipient_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Прибув у відділення");
        }

        private void Chb_WaitingForSending_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Готується до відправлення");
        }

        private void Chb_Proccessing_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Замовлення в обробці");
        }

        private void Chb_delievered_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _stateFilter.Add("Одержаний");
        }
        #endregion

        #region DataGrid events

        /// <summary>
        /// getDocumentList  give Created stateName
        /// documentTracking give just in time stateName 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Track_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = DG_doc.SelectedItems.Cast<DataItem>().ToList();

            var methodPrepetries = CreateXmlListPropertiesForDocumentsTracking(selectedItems);

            OpenLoader("InternetDocument", "documentsTracking", methodPrepetries);


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                int i = 0;

                foreach (XmlNode item in methodPrepetries[0].ChildNodes)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM [TTN] WHERE (TTN = @TTN)", connection))
                    {
                        cmd.Parameters.AddWithValue("@TTN", item.InnerText);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                while (await reader.ReadAsync())
                                {
                                    var dataItem = new DataItem();
                                    dataItem.IntDocNumber = reader.GetString(0).Trim();
                                    dataItem.DateTime = reader.GetDateTime(1);
                                    dataItem.CityRecipientDescription = reader.GetString(2).Trim();
                                    dataItem.RecipientDescription = reader.GetString(3).Trim();
                                    dataItem.RecipientAddressDescription = reader.GetString(4).Trim();
                                    dataItem.RecipientContactPhone = reader.GetString(5).Trim();
                                    dataItem.Weight = reader.GetDouble(6);
                                    dataItem.Cost = reader.GetDouble(7);
                                    dataItem.CostOnSite = reader.GetDouble(8);
                                    dataItem.StateName = reader.GetString(9).Trim();
                                    dataItem.PrintedDescription = reader.GetString(10).Trim();

                                    var index = DG_doc.Items.IndexOf(selectedItems[i]);
                                    DG_doc.Items[index] = dataItem;
                                    i++;
                                }
                        }
                    }
                }
            }

        }

        private static XmlNodeList CreateXmlListPropertiesForDocumentsTracking(List<DataItem> selectItems)
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

        #endregion
    }
}

