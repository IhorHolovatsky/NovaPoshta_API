using System;
using System.Collections.Generic;
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
        private Task _findTask;
        private readonly string _apiKey;
        private readonly string _loadMethod;
        private readonly IsolatedStorageFile _isolated = IsolatedStorageFile.GetUserStoreForAssembly();
        private DateBaseOfDocuments _dbDocs;


        public Loading(string apiKey, string loadMethod)
        {
            InitializeComponent();
            _apiKey = apiKey;
            _loadMethod = loadMethod;
            pb_state.Maximum = 100.0;
        }

        private async void Loading_OnLoaded(object sender, RoutedEventArgs e)
        {
            l_state.Content = "Перевірка з'єднання з інтернетом...";

            bool isInternetConnection = await CheckConnectionAsync();
            if (!isInternetConnection)
            {
                MessageBox.Show("No Internet Connection!");
                this.Close();
            }
            l_state.Content = "Запит обробляється...";

            //Methods in web api
            switch (_loadMethod)
            {
                case "RefreshDataBase":
                    await RefreshDataBase();
                    break;
            } 
        }

        private async Task RefreshDataBase()
        {
            DateTime left;
            DateTime right;

            var fileNames = _isolated.GetFileNames();

            if (fileNames.Length == 0)
                left = DateTime.Parse("01.01.2015");
            else
            {
                var fileName = fileNames.Single(fName => fName == _apiKey);
                if (fileName.Length != 0)
                {
                    //Deserialization;
                    _dbDocs = await Task.Factory.StartNew(() => DeSerializeDocuments(fileName));
                    left = _dbDocs.Dates.Max;
                }
                else
                {
                    left = DateTime.Parse("01.01.2015");
                }
            }

            right = DateTime.Today;

            if (_dbDocs == null)
                _dbDocs = new DateBaseOfDocuments();

            _cts = new CancellationTokenSource();
            try
            {
                _findTask = GetNovaPoshtaDocuments(left, right);
                await _findTask;
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

            await Task.Factory.StartNew(() => SerializeDocuments(_apiKey, _dbDocs));

            this.Close();
        }
       
        private async Task GetNovaPoshtaDocuments(DateTime left, DateTime right)
        {

            var makeTasks = await Task<IEnumerable<Task<XmlDocument>>>.Factory.StartNew(
                () =>
                {
                    IEnumerable<XmlNodeList> xmlQueryProperties = from x in Enumerable.Range(0, (right - left).Days + 1)
                                                                  select CreateXmlListPropertiesForGetDocuments(left, x);

                    IEnumerable<Task<XmlDocument>> tasks = from x in xmlQueryProperties
                                                           select MakeTask("InternetDocument", "getDocumentList", x);

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

                        b.Remove(task.Result);

                        AsyncChangeControlState(pb_state, () => pb_state.Value += 100.0 / ((right - left).Days + 1));

                        if (_cts.IsCancellationRequested)
                            _cts.Token.ThrowIfCancellationRequested();
                    }
                });
        }

        private async Task<XmlDocument> MakeTask(string modelName, string methodName, XmlNodeList xmlList)
        {
            var newDocument = new Document();
            var xmlQuery = newDocument.MakeRequestXmlDocument(_apiKey, modelName, methodName, xmlList);

            XmlDocument xmlResponse = null;
            try
            {
                Thread.Sleep(new Random().Next(50));
                xmlResponse = await newDocument.SendRequestXmlDocumentAsync(xmlQuery);
            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message);
                Thread.CurrentThread.Abort();
            }

            newDocument.LoadResponseXmlDocument(xmlResponse);
            _dbDocs.Add(newDocument);

            if (!newDocument.Success)
            {
                MessageBox.Show("Invalid API key");
                Thread.CurrentThread.Abort();
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

        private void  SerializeDocuments(string fileName, DateBaseOfDocuments dbDocs)
        {
            var fileStream = _isolated.OpenFile(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            //var dataContractSerializer = new DataContractSerializer(typeof(List<Document>));
            //var xmlStream = XmlWriter.Create(a);
            //dataContractSerializer.WriteObject(xmlStream, responseDocuments);
            //xmlStream.Close();

            var binnaryFormatter = new BinaryFormatter();
            binnaryFormatter.Serialize(fileStream, dbDocs);

            fileStream.Close();
        }

        private  DateBaseOfDocuments DeSerializeDocuments(string fileName)
        {
            var stream = _isolated.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Write);
            var binnaryFormatter = new BinaryFormatter();

            var responseList = binnaryFormatter.Deserialize(stream) as DateBaseOfDocuments;
            stream.Close();
            
            return responseList;
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


        private void Btn_cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if (_findTask == null) return;
            if (_findTask.IsCompleted) return;

            _cts.Cancel();
        }
    }

}
