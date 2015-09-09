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
        private APImethods _apiMethods;
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
            _apiMethods = new APImethods(apiKey);
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
            else
            {


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
                    case "getCities":
                        await GetCities();
                        break;
                    case "RefreshLibraries":
                        await GetCities();

                        break;

                }
            }
            this.Close();
        }

        #region getDocumentList

        private async Task GetDocumentList()
        {
            DateTime left;
            DateTime right;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT max(DateTime) FROM [TTN]", connection))
            {
                await connection.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    try
                    {
                        await reader.ReadAsync();
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

        }

        private async Task _GetDocumentList(DateTime left, DateTime right)
        {

            var makeTasks = await Task<IEnumerable<Task<Document<DataItem>>>>.Factory.StartNew(
                () =>
                {
                    var xmlQueryProperties = from x in Enumerable.Range(0, (right - left).Days + 1)
                                             select CreateXmlListPropertiesForGetDocuments(left, x);

                    var tasks = from x in xmlQueryProperties
                                select _apiMethods.GetDocumentListAsync(x);

                    return tasks;
                }
                );

            var b = makeTasks.ToList();

            while (b.Count > 0)
            {
                var task = await Task.WhenAny(b);

                if (_cts.IsCancellationRequested)
                    _cts.Token.ThrowIfCancellationRequested();

                b.Remove(task);

                pb_state.Value += 100.0 / ((right - left).Days + 1);

                var doc = task.Result;

                if (!doc.HasData) continue;
                if (!doc.Success)
                {
                    MessageBox.Show("Invalid API key");
                    Close();
                }

                lock (_locker)
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        foreach (var item in doc.Items)
                        {
                            AddToDateBase(connection, item);
                        }
                    }
                }
            }

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
                await _DocumentsTracking();
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

        }

        private async Task _DocumentsTracking()
        {
            var document = await _apiMethods.DocumentsTrackingAsync(_methodProperties);


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var i = document.Items.Count;
                foreach (var item in document.Items)
                {
                    using (
                        SqlCommand cmd =
                            new SqlCommand(
                                "UPDATE [TTN] SET StateName = @StateName WHERE TTN = @TTN", connection))
                    {
                        cmd.Parameters.AddWithValue("@TTN", item.Barcode);
                        cmd.Parameters.AddWithValue("@StateName", item.StateName);

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (SqlException e)
                        {
                            return;
                        }

                        pb_state.Value += 100.0 / i;
                    }
                }
            }
        }


        #endregion

        #region getCities

        private async Task GetCities()
        {
            var doc = await _apiMethods.GetCitiesAsync(_methodProperties);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await AddCitiesToDataBase(doc, connection);
            }
        }
        private async Task AddCitiesToDataBase(Document<City> doc, SqlConnection connection)
        {
            foreach (var item in doc.Items)
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand(
                            "INSERT INTO [Cities] (Description, DescriptionRu, Ref, Monday, Tuesday, Wednesday," +
                            " Thursday, Friday, Saturday, Sunday, Area, CityID) VALUES " +
                            "(@Description, @DescriptionRu, @Ref, @Monday, @Tuesday, @Wednesday," +
                            " @Thursday, @Friday, @Saturday, @Sunday, @Area, @CityID)", connection))
                {
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                    cmd.Parameters.AddWithValue("@DescriptionRu", item.DescriptionRu);
                    cmd.Parameters.AddWithValue("@Ref", item.Ref);
                    cmd.Parameters.AddWithValue("@Monday", item.Monday);
                    cmd.Parameters.AddWithValue("@Tuesday", item.Tuesday);
                    cmd.Parameters.AddWithValue("@Wednesday", item.Wednesday);
                    cmd.Parameters.AddWithValue("@Thursday", item.Thursday);
                    cmd.Parameters.AddWithValue("@Friday", item.Friday);
                    cmd.Parameters.AddWithValue("@Saturday", item.Saturday);
                    cmd.Parameters.AddWithValue("@Sunday", item.Sunday);
                    cmd.Parameters.AddWithValue("@Area", item.Area);
                    cmd.Parameters.AddWithValue("@CityID", item.CityId);
                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqlException e)
                    {
                        return;
                    }
                }
            }
        }

        #endregion

        #region getStreet
        #endregion

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
