using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    public class City : IComponent
    {
        private string _description;
        private string _descriptionRu;
        private string _ref;
        private bool _monday;
        private bool _tuesday;
        private bool _wednesday;
        private bool _thursday;
        private bool _friday;
        private bool _saturday;
        private bool _sunday;
        private string _area;
        private string _cityID;


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string DescriptionRu
        {
            get { return _descriptionRu; }
            set { _descriptionRu = value; }
        }

        public string Ref
        {
            get { return _ref; }
            set { _ref = value; }
        }

        public bool Monday
        {
            get { return _monday; }
            set { _monday = value; }
        }

        public bool Tuesday
        {
            get { return _tuesday; }
            set { _tuesday = value; }
        }

        public bool Wednesday
        {
            get { return _wednesday; }
            set { _wednesday = value; }
        }

        public bool Thursday
        {
            get { return _thursday; }
            set { _thursday = value; }
        }

        public bool Friday
        {
            get { return _friday; }
            set { _friday = value; }
        }

        public bool Saturday
        {
            get { return _saturday; }
            set { _saturday = value; }
        }

        public bool Sunday
        {
            get { return _sunday; }
            set { _sunday = value; }
        }

        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }

        public string CityId
        {
            get { return _cityID; }
            set { _cityID = value; }
        }


        public void LoadFromXml(XmlNode xmlDoc)
        {
            foreach (XmlNode info in xmlDoc.ChildNodes)
            {
                int x;

                switch (info.Name)
                {
                    case "Description":
                        _description = info.InnerText;
                        break;
                    case "DescriptionRu":
                        _descriptionRu = info.InnerText;
                        break;
                    case "Ref":
                        _ref = info.InnerText;
                        break;
                    case "Delivery1":
                        x = Int32.Parse(info.InnerText);
                        _monday = x != 0;
                        break;
                    case "Delivery2":
                        x = Int32.Parse(info.InnerText);
                        _tuesday = x != 0;
                        break;
                    case "Delivery3":
                        x = Int32.Parse(info.InnerText);
                        _wednesday = x != 0;
                        break;
                    case "Delivery4":
                        x = Int32.Parse(info.InnerText);
                        _thursday = x != 0;
                        break;
                    case "Delivery5":
                        x = Int32.Parse(info.InnerText);
                        _friday = x != 0;
                        break;
                    case "Delivery6":
                        x = Int32.Parse(info.InnerText);
                        _saturday = x != 0;
                        break;
                    case "Delivery7":
                        x = Int32.Parse(info.InnerText);
                        _sunday = x != 0;
                        break;
                    case "Area":
                        _area = info.InnerText;
                        break;
                    case "CityID":
                        _cityID = info.InnerText;
                        break;
                }
            }
        }
    }
}
