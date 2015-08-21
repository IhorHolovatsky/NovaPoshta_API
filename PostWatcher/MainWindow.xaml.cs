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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private static string _APIKey;
        private static string _modelName;
        private static string _methodName;
        private static DataItem filter = new DataItem();
        private static List<string> stateFilter = new List<string>(10);
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

            if (rk == null)
            {
                rk = Registry.CurrentUser.CreateSubKey("PostWatcher");
                var newWindwowApIkey = new APIkey();
                newWindwowApIkey.ShowDialog();
            }
            else
            {
                if (rk.GetValue("API key") == null)
                {
                    var newWindwowApIkey = new APIkey();
                    newWindwowApIkey.ShowDialog();
                }
            }

            _APIKey = (string)rk.GetValue("API key");

            DG_doc.Items.Clear();
            await Task.Factory.StartNew(GetFiles);
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

        private void SaveRequest(XmlDocument xmlDocument, string name)
        {
            lock (locker)
            {
                var writer = new XmlTextWriter(_isolated.CreateFile("Request" + name + ".xml"), Encoding.UTF8);
                xmlDocument.Save(writer);
                writer.Close();
            }
        }

        private void GetFiles()
        {
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
            document.LoadResponseXmlDocument(xmlDocument);

            if (!document.Success || !document.HasData)
                return;

            foreach (var dataItem in document.Items)
            {
                if (!CompareTwoDocuments(dataItem, filterDocument)) continue;
                if (stateFilter.Contains(dataItem.StateName)) continue;

                var thisValue = dataItem;
                AsyncChangeControlState(DG_doc, () => DG_doc.Items.Add(thisValue));
            }
        }

        private static bool CompareTwoDocuments(DataItem self, DataItem to)
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
            if (findTask == null) return;
            if (findTask.IsCompleted) return;

            cts.Cancel();
        }

        private async void Btn_OKfilter_OnClick(object sender, RoutedEventArgs e)
        {
            DG_doc.Items.Clear();
            btn_OKfilter.IsEnabled = false;
            await Task.Factory.StartNew(GetFiles);
            btn_OKfilter.IsEnabled = true;
        }

        private async void Btn_selectDataOK_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime left = DatePickerLeft.SelectedDate ?? DateTime.Today;
            DateTime right = DatePickerRight.SelectedDate ?? DateTime.Today;
            prb_state.Value = 0;

            btn_selectDataOK.IsEnabled = false;
            cts = new CancellationTokenSource();
            try
            {
                findTask = _GetNovaPoshtaDocuments(left, right);
                await findTask;
            }
            catch (OperationCanceledException exception)
            {
                tb_state.Text = "Відмінено";
                prb_state.Visibility = Visibility.Hidden;
            }
            finally
            {
                cts.Dispose();
            }

        btn_selectDataOK.IsEnabled = true;
        }
        private async Task _GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
            tb_state.Text = "Перевірка з'єднання з інтернетом...";

            
            bool isInternetConnection = await CheckConnectionAsync();
            if (!isInternetConnection)
            {
                MessageBox.Show("No Internet Connection!");
                tb_state.Text = "No Internet Connection!";
                return;
            }
            tb_state.Text = "Запит обробляється...";

            await Task.Factory.StartNew(() =>
              {
                  foreach (var file in _isolated.GetFileNames())
                  {
                      _isolated.DeleteFile(file);
                  }

              });

            var timer = new Stopwatch();
            timer.Start();

            prb_state.Visibility = Visibility.Visible;
            await GetNovaPoshtaDocuments(left, right);
            prb_state.Visibility = Visibility.Hidden;

            timer.Stop();

            tb_state.Text = "Затрачений час: " + timer.Elapsed.ToString("g");
        }

        private async Task GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {
            DG_doc.Items.Clear();

            _modelName = "InternetDocument";
            _methodName = "getDocumentList";

            var makeTasks = await Task<IEnumerable<Task<XmlDocument>>>.Factory.StartNew(
                () =>
                {
                    IEnumerable<Task<XmlDocument>> a = from x in Enumerable.Range(0, (right - left).Days + 1)
                                                       select MakeTask(left, x);
                    return a;
                }
                );

            await Task.Factory.StartNew(
                () =>
                {
                    List<Task<XmlDocument>> b = makeTasks.ToList();

                    while (b.Count > 0)
                    {
                        var task = Task.WhenAny(b);

                        b.Remove(task.Result);
                        AddItemsToDataGrid(task.Result.Result, filter);

                        AsyncChangeControlState(prb_state, () => prb_state.Value += 1000.0 / ((right - left).Days + 1));

                        if (cts.IsCancellationRequested)
                            cts.Token.ThrowIfCancellationRequested();
                    }
                });
        }

        private async Task<XmlDocument> MakeTask(DateTime left, int i)
        {

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
                xmlResponse = await newDocument.SendRequestXmlDocumentAsync(xmlQuery);


            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message);
                AsyncChangeControlState(prb_state, () => prb_state.Visibility = Visibility.Hidden);
                Thread.CurrentThread.Abort();
            }

            await Task.Factory.StartNew(() => SaveRequest(xmlResponse, i.ToString()));
            newDocument.LoadResponseXmlDocument(xmlResponse);

            if (!newDocument.Success)
            {
                MessageBox.Show("Invalid API key");
                AsyncChangeControlState(prb_state, () => prb_state.Visibility = Visibility.Hidden);
                Thread.CurrentThread.Abort();
            }

            return xmlResponse;
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

    }
}

