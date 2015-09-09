using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    public class Week : IComponent
    {
        private string _monday;
        private string _tuesday;
        private string _wednesday;
        private string _thursday;
        private string _friday;
        private string _saturday;
        private string _sunday;

        public string Monday
        {
            get { return _monday; }
            set { _monday = value; }
        }

        public string Tuesday
        {
            get { return _tuesday; }
            set { _tuesday = value; }
        }

        public string Wednesday
        {
            get { return _wednesday; }
            set { _wednesday = value; }
        }

        public string Thursday
        {
            get { return _thursday; }
            set { _thursday = value; }
        }

        public string Friday
        {
            get { return _friday; }
            set { _friday = value; }
        }

        public string Saturday
        {
            get { return _saturday; }
            set { _saturday = value; }
        }

        public string Sunday
        {
            get { return _sunday; }
            set { _sunday = value; }
        }

        public void LoadFromXml(XmlNode doc)
        {
            foreach (XmlNode info in doc.ChildNodes)
            {

                switch (info.Name)
                {
                    case "Monday":
                        _monday = info.InnerText;
                        break;
                    case "Tuesday":
                        _tuesday = info.InnerText;
                        break;
                    case "Wednesday":
                        _wednesday = info.InnerText;
                        break;
                    case "Thursday":
                        _thursday = info.InnerText;
                        break;
                    case "Friday":
                        _friday = info.InnerText;
                        break;
                    case "Saturday":
                        _saturday = info.InnerText;
                        break;
                    case "Sunday":
                        _sunday = info.InnerText;
                        break;
                }
            }
        }
    }
}
