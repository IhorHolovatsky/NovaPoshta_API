using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PostWatcher
{
    [Serializable]
    class DateBaseOfDocuments
    {
        private List<Document> _documents = new List<Document>();
        private SortedSet<DateTime> _dates = new SortedSet<DateTime>(); 
        public SortedSet<DateTime> Dates
        {
            get { return _dates; }
            private set { }
        }

        public List<Document> Documents
        {
            get
            {
                return _documents;
            }
            set { }
        }
        public DateBaseOfDocuments()
        {

        }
        public DateBaseOfDocuments(List<Document> docs)
        {
            Add(docs);
        }

        public void Add(Document doc)
        {
            if (!doc.HasData)
                return;

            if (_dates.Contains(doc.Date))
                return;

            _dates.Add(doc.Date);
            _documents.Add(doc);
        }

        public void Add(List<Document> docs)
        {
            foreach (var doc in docs)
            {
                Add(doc);
            }
        }

        public void Resfresh(Document doc)
        {
            var i = _documents.IndexOf(doc);

            if (i == -1)
                return;

            _documents[i] = doc;
        }

        public void Refresh(List<Document> docs)
        {
            foreach (var doc in docs)
            {
                Resfresh(doc);
            }
        }
    }
}
