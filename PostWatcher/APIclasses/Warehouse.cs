using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    public class Warehouse : IComponent
    {
        private string _description;
        private string _descriptionRu;
        private string _phone;
        private string _typeOfWarehouse;
        private string _ref;
        private string _number;
        private string _cityRef;
        private string _maxWeightAllowed;
        private string _longitude;
        private string _latitude;
        private Week _reception;
        private Week _delivery;
        private Week _schedule;

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

        public string TypeOfWarehouse
        {
            get { return _typeOfWarehouse; }
            set { _typeOfWarehouse = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Ref
        {
            get { return _ref; }
            set { _ref = value; }
        }

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public string CityRef
        {
            get { return _cityRef; }
            set { _cityRef = value; }
        }

        public string MaxWeightAllowed
        {
            get { return _maxWeightAllowed; }
            set { _maxWeightAllowed = value; }
        }

        public string Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public Week Reception
        {
            get { return _reception; }
            set { _reception = value; }
        }

        public Week Delivery
        {
            get { return _delivery; }
            set { _delivery = value; }
        }

        public Week Schedule
        {
            get { return _schedule; }
            set { _schedule = value; }
        }

        public void LoadFromXml(XmlNode doc)
        {
            foreach (XmlNode info in doc.ChildNodes)
            {
                switch (info.Name)
                {
                    case "Description":
                        _description = info.InnerText;
                        break;
                    case "DescriptionRu":
                        _descriptionRu = info.InnerText;
                        break;
                    case "Phone":
                        _phone = info.InnerText;
                        break;
                    case "TypeOfWarehouse":
                        _typeOfWarehouse = info.InnerText;
                        break;
                    case "Ref":
                        _ref = info.InnerText;
                        break;
                    case "Number":
                        _number = info.InnerText;
                        break;
                    case "CityRef":
                        _cityRef = info.InnerText;
                        break;
                    case "MaxWeightAllowed":
                        _maxWeightAllowed = info.InnerText;
                        break;
                    case "Longitude":
                        _longitude = info.InnerText;
                        break;
                    case "Latitude":
                        _latitude = info.InnerText;
                        break;
                    case "Reception":
                        _reception = new Week();
                        _reception.LoadFromXml(info);
                        break;
                    case "Delivery":
                        _delivery = new Week();
                        _delivery.LoadFromXml(info);
                        break;
                    case "Schedule":
                        _schedule = new Week();
                        _schedule.LoadFromXml(info);
                        break;
                }
            }


        }
    }
}
