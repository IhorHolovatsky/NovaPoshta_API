using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;

namespace PostWatcher
{
    /// <summary>
    /// Window that will show status of loading data
    /// </summary>
    public partial class Loading
    {
        private CancellationTokenSource _cts;
        private Task _runnedTask;
        private readonly string _apiKey;
        private readonly string _modelName;
        private readonly string _methodName;
        private readonly XmlNodeList _methodProperties;
        private static string _connectionString;
        private readonly object _locker = new object();

        public Loading(string apiKey, string modelName, string methodName, XmlNodeList methodProperties)
        {
            InitializeComponent();
            _apiKey = apiKey;
            _modelName = modelName;
            _methodName = methodName;
            _methodProperties = methodProperties;
            pb_state.Maximum = 100.0;
        }

        private async void Loading_OnLoaded(object sender, RoutedEventArgs e)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectToTTN"].ConnectionString;

            l_state.Content = "Перевірка з'єднання з інтернетом...";

            bool isInternetConnection = await CheckConnectionAsync();
            if (!isInternetConnection)
            {
                MessageBox.Show("No Internet Connection!");
                this.Close();
            }
            l_state.Content = "Запит обробляється...";

            //Methods in web api
            switch (_methodName)
            {
                case "getDocumentList":
                    await GetDocumentList();
                    break;
                case "documentsTracking":
                    await DocumentsTracking();
                    break;

            }
        }

        #region getDocumentList

        private async Task GetDocumentList()
        {
            DateTime left;
            DateTime right;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT max(DateTime) FROM [TTN]", connection))
            {
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        reader.Read();
                        left = reader.GetDateTime(0);
                    }
                    catch (Exception)
                    {
                        left = DateTime.Parse("01.01.2015");
                    }
                }
            }

            right = DateTime.Today;

            _cts = new CancellationTokenSource();

            try
            {
                _runnedTask = _GetDocumentList(left, right);
                await _runnedTask;
            }
            catch (OperationCanceledException)
            {
                l_state.Content = "Відмінено";
                this.Close();
            }
            finally
            {
              _cts.Dispose();
            }

            this.Close();
        }

        private async Task _GetDocumentList(DateTime left, DateTime right)
        {

            var makeTasks = await Task<IEnumerable<Task<XmlDocument>>>.Factory.StartNew(
                () =>
                {
                    IEnumerable<XmlNodeList> xmlQueryProperties = from x in Enumerable.Range(0, (right - left).Days + 1)
                                                                  select CreateXmlListPropertiesForGetDocuments(left, x);

                    IEnumerable<Task<XmlDocument>> tasks = from x in xmlQueryProperties
                                                           select MakeTask(_modelName, _methodName, x);

                    return tasks;
                }
                );

            await Task.Factory.StartNew(
                () =>
                {
                    List<Task<XmlDocument>> b = makeTasks.ToList();

                    while (b.Count > 0)
                    {
                        var task = Task.WhenAny(b);
                        var xmlResponse = task.Result.Result;
                       
                        if (_cts.IsCancellationRequested)
                            _cts.Token.ThrowIfCancellationRequested();

                        b.Remove(task.Result);

                        AsyncChangeControlState(pb_state, () => pb_state.Value += 100.0 / ((right - left).Days + 1));
                        
                        var newDocument = new Document();
                        newDocument.LoadResponseXmlDocument(xmlResponse);

                        if (!newDocument.HasData) continue;
                        if (!newDocument.Success)
                        {
                            MessageBox.Show("Invalid API key");
                            Close();
                        }
                        
                        lock (_locker)
                        {
                            using (SqlConnection connection = new SqlConnection(_connectionString))
                            {
                                connection.Open();
                                foreach (var item in newDocument.Items)
                                {
                                    AddToDateBase(connection, item);
                                }
                            }
                        }


                   
                   
                    }
                });
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

        private void AddToDateBase(SqlConnection connection, DataItem item)
        {
            using (
                SqlCommand cmd =
                    new SqlCommand(
                        "INSERT INTO [TTN] (TTN, DateTime, CityRecipientDescription, RecipientDescription, " +
                        "RecipientAddressDescription, RecipientContactPhone, Weight, Cost, CostOnSite, StateName," +
                        " PrintedDescription, APIKey) VALUES (@TTN, @DateTime, @CityRecipientDescription, @RecipientDescription, " +
                        "@RecipientAddressDescription, @RecipientContactPhone, @Weight, @Cost, @CostOnSite, @StateName," +
                        "@PrintedDescription, @APIKey)", connection))
            {
                cmd.Parameters.AddWithValue("@TTN", item.IntDocNumber);
                cmd.Parameters.AddWithValue("@DateTime", item.DateTime);
                cmd.Parameters.AddWithValue("@CityRecipientDescription", item.CityRecipientDescription);
                cmd.Parameters.AddWithValue("@RecipientDescription", item.RecipientDescription);
                cmd.Parameters.AddWithValue("@RecipientAddressDescription", item.RecipientAddressDescription);
                cmd.Parameters.AddWithValue("@RecipientContactPhone", item.RecipientContactPhone);
                cmd.Parameters.AddWithValue("@Weight", item.Weight);
                cmd.Parameters.AddWithValue("@Cost", item.Cost);
                cmd.Parameters.AddWithValue("@CostOnSite", item.CostOnSite);
                cmd.Parameters.AddWithValue("@StateName", item.StateName);
                cmd.Parameters.AddWithValue("@PrintedDescription", item.PrintedDescription);
                cmd.Parameters.AddWithValue("@APIKey", _apiKey);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    return;
                }
            }
        }

        #endregion

        #region documentsTracking
        private async Task DocumentsTracking()
        {
            _cts = new CancellationTokenSource();

            try
            {
                _runnedTask = _DocumentsTracking();
                await _runnedTask;
            }
            catch (OperationCanceledException)
            {
                l_state.Content = "Відмінено";
                this.Close();
            }
            finally
            {
                _cts.Dispose();
            }

            this.Close();
        }

        private async Task _DocumentsTracking()
        {
            var task = await MakeTask(_modelName, _methodName, _methodProperties);

            var document = new Document();
            document.LoadResponseXmlDocument(task);


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var item in document.Items)
                {
                    using (
                        SqlCommand cmd =
                            new SqlCommand(
                                "UPDATE [TTN] SET StateName = @StateName WHERE TTN = @TTN", connection))
                    {
                        cmd.Parameters.AddWithValue("@TTN", item.IntDocNumber);
                        cmd.Parameters.AddWithValue("@StateName", item.StateName);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            return;
                        }
                    }
                }
            }



        }

        
        #endregion


        private async Task<XmlDocument> MakeTask(string modelName, string methodName, XmlNodeList xmlList)
        {
           
            var xmlQuery = Document.MakeRequestXmlDocument(_apiKey, modelName, methodName, xmlList);

            XmlDocument xmlResponse = null;
            try
            {
                Thread.Sleep(new Random().Next(50));
                xmlResponse = await Document.SendRequestXmlDocumentAsync(xmlQuery);
            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message);
                Close();
            }

            return xmlResponse;
        }


        private void AsyncChangeControlState(Control element, Action action)
        {
            if (element.Dispatcher.CheckAccess())
                action();
            else
                element.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    action);
        }

        protected async Task<bool> CheckConnectionAsync()
        {
            try
            {

                using (var client = new WebClient())
                using (await client.OpenReadTaskAsync("http://www.google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }



        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (_runnedTask == null) return;
            if (_runnedTask.IsCompleted) return;

            _cts.Cancel();
        }
    }

}
