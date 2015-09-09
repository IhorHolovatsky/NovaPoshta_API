using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    public class TrackingItem : IComponent
    {
        private string _barcode;
        private string _stateId;
        private string _stateName;
        private string _checkWeight;
        private string _documentCost;
        private DateTime _dataReceived;
        private string _recipientFullName;
        private string _recipientPost;
        private DateTime _receiptDateTime;
        private bool _onlinePayment;
        private string _deliveryFrom;
        private string _addressUa;
        private string _addressRu;
        private string _wareReceiverId;
        private string _backDelivery;
        private string _redeliveryNum;
        private string _cityReceiverSiteKey;
        private string _cityReceiverUa;
        private string _cityReceiverRu;
        private string _citySenderSiteKey;
        private string _citySenderUa;
        private string _citySenderRu;
        private string _deliveryType;
        private string _backwardDeliveryNumber;
        private string _redeliveryCargoDescriptionMoney;
        private bool _failure;
        private string _reasonDescription;
        private bool _globalMoneyExistDelivery;
        private string _globalMoneyLastTransactionStatus;
        private DateTime _globalMoneyLastTransactionDate;
        private string _sum;
        private string _documentWeight;
        private string _sumBeforeCheckWeight;
        private bool _isEwpPaid;
        private string _isEwpPaidCashLess;
        private string _ewPaidSumm;
        private string _redeliverySum;
        private string _ownerDocumentType;
        private string _childDocuments;


        public string Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }

        public string StateId
        {
            get { return _stateId; }
            set { _stateId = value; }
        }

        public string StateName
        {
            get { return _stateName; }
            set { _stateName = value; }
        }

        public string CheckWeight
        {
            get { return _checkWeight; }
            set { _checkWeight = value; }
        }

        public string DocumentCost
        {
            get { return _documentCost; }
            set { _documentCost = value; }
        }

        public DateTime DataReceived
        {
            get { return _dataReceived; }
            set { _dataReceived = value; }
        }

        public string RecipientFullName
        {
            get { return _recipientFullName; }
            set { _recipientFullName = value; }
        }

        public string RecipientPost
        {
            get { return _recipientPost; }
            set { _recipientPost = value; }
        }

        public DateTime ReceiptDateTime
        {
            get { return _receiptDateTime; }
            set { _receiptDateTime = value; }
        }

        public bool OnlinePayment
        {
            get { return _onlinePayment; }
            set { _onlinePayment = value; }
        }

        public string DeliveryFrom
        {
            get { return _deliveryFrom; }
            set { _deliveryFrom = value; }
        }

        public string AddressUa
        {
            get { return _addressUa; }
            set { _addressUa = value; }
        }

        public string AddressRu
        {
            get { return _addressRu; }
            set { _addressRu = value; }
        }

        public string WareReceiverId
        {
            get { return _wareReceiverId; }
            set { _wareReceiverId = value; }
        }

        public string BackDelivery
        {
            get { return _backDelivery; }
            set { _backDelivery = value; }
        }

        public string RedeliveryNum
        {
            get { return _redeliveryNum; }
            set { _redeliveryNum = value; }
        }

        public string CityReceiverSiteKey
        {
            get { return _cityReceiverSiteKey; }
            set { _cityReceiverSiteKey = value; }
        }

        public string CityReceiverUa
        {
            get { return _cityReceiverUa; }
            set { _cityReceiverUa = value; }
        }

        public string CityReceiverRu
        {
            get { return _cityReceiverRu; }
            set { _cityReceiverRu = value; }
        }

        public string CitySenderSiteKey
        {
            get { return _citySenderSiteKey; }
            set { _citySenderSiteKey = value; }
        }

        public string CitySenderUa
        {
            get { return _citySenderUa; }
            set { _citySenderUa = value; }
        }

        public string CitySenderRu
        {
            get { return _citySenderRu; }
            set { _citySenderRu = value; }
        }

        public string DeliveryType
        {
            get { return _deliveryType; }
            set { _deliveryType = value; }
        }

        public string BackwardDeliveryNumber
        {
            get { return _backwardDeliveryNumber; }
            set { _backwardDeliveryNumber = value; }
        }

        public string RedeliveryCargoDescriptionMoney
        {
            get { return _redeliveryCargoDescriptionMoney; }
            set { _redeliveryCargoDescriptionMoney = value; }
        }

        public bool Failure
        {
            get { return _failure; }
            set { _failure = value; }
        }

        public string ReasonDescription
        {
            get { return _reasonDescription; }
            set { _reasonDescription = value; }
        }

        public bool GlobalMoneyExistDelivery
        {
            get { return _globalMoneyExistDelivery; }
            set { _globalMoneyExistDelivery = value; }
        }

        public string GlobalMoneyLastTransactionStatus
        {
            get { return _globalMoneyLastTransactionStatus; }
            set { _globalMoneyLastTransactionStatus = value; }
        }

        public DateTime GlobalMoneyLastTransactionDate
        {
            get { return _globalMoneyLastTransactionDate; }
            set { _globalMoneyLastTransactionDate = value; }
        }

        public string Sum
        {
            get { return _sum; }
            set { _sum = value; }
        }

        public string DocumentWeight
        {
            get { return _documentWeight; }
            set { _documentWeight = value; }
        }

        public string SumBeforeCheckWeight
        {
            get { return _sumBeforeCheckWeight; }
            set { _sumBeforeCheckWeight = value; }
        }

        public bool IsEwpPaid
        {
            get { return _isEwpPaid; }
            set { _isEwpPaid = value; }
        }

        public string IsEwpPaidCashLess
        {
            get { return _isEwpPaidCashLess; }
            set { _isEwpPaidCashLess = value; }
        }

        public string EwPaidSumm
        {
            get { return _ewPaidSumm; }
            set { _ewPaidSumm = value; }
        }

        public string RedeliverySum
        {
            get { return _redeliverySum; }
            set { _redeliverySum = value; }
        }

        public string OwnerDocumentType
        {
            get { return _ownerDocumentType; }
            set { _ownerDocumentType = value; }
        }

        public string ChildDocuments
        {
            get { return _childDocuments; }
            set { _childDocuments = value; }
        }

        public void LoadFromXml(XmlNode doc)
        {
            foreach (XmlNode info in doc.ChildNodes)
            {
                switch (info.Name)
                {
                    case "Barcode":
                        _barcode = info.InnerText;
                        break;
                    case "StateId":
                        _stateId = info.InnerText;
                        break;
                    case "StateName":
                        _stateName = info.InnerText;
                        break;
                    case "CheckWeight":
                        _checkWeight = info.InnerText;
                        break;
                    case "DocumentCost":
                        _documentCost = info.InnerText;
                        break;
                    case "DataReceived":
                        _dataReceived = DateTime.Parse(info.InnerText);
                        break;
                    case "RecipientFullName":
                        _recipientFullName = info.InnerText;
                        break;
                    case "RecipientPost":
                        _recipientPost = info.InnerText;
                        break;
                    case "ReceiptDateTime":
                        _receiptDateTime = DateTime.Parse(info.InnerText);
                        break;

                    case "OnlinePayment":
                        _onlinePayment = Boolean.Parse(info.InnerText);
                        break;
                    case "DeliveryFrom":
                        _deliveryFrom = info.InnerText;
                        break;
                    case "AddressUa":
                        _addressUa = info.InnerText;
                        break;
                    case "AddressRu":
                        _addressRu = info.InnerText;
                        break;
                    case "WareReceiverId":
                        _wareReceiverId = info.InnerText;
                        break;
                    case "BackDelivery":
                        _backDelivery = info.InnerText;
                        break;
                    case "RedeliveryNum":
                        _redeliveryNum = info.InnerText;
                        break;
                    case "CityReceiverSiteKey":
                        _cityReceiverSiteKey = info.InnerText;
                        break;
                    case "CityReceiverUa":
                        _cityReceiverUa = info.InnerText;
                        break;
                    case "CityReceiverRu":
                        _cityReceiverRu = info.InnerText;
                        break;
                    case "CitySenderSiteKey":
                        _citySenderSiteKey = info.InnerText;
                        break;
                    case "CitySenderUa":
                        _citySenderUa = info.InnerText;
                        break;
                    case "CitySenderRu":
                        _citySenderRu = info.InnerText;
                        break;
                    case "DeliveryType":
                        _deliveryType = info.InnerText;
                        break;
                    case "BackwardDeliveryNumber":
                        _backwardDeliveryNumber = info.InnerText;
                        break;
                    case "RedeliveryCargoDescriptionMoney":
                        _redeliveryCargoDescriptionMoney = info.InnerText;
                        break;
                    case "Failure":
                        _failure = Boolean.Parse(info.InnerText);
                        break;
                    case "ReasonDescription":
                        _reasonDescription = info.InnerText;
                        break;
                    case "GlobalMoneyExistDelivery":
                        _globalMoneyExistDelivery = Boolean.Parse(info.InnerText);
                        break;
                    case "GlobalMoneyLastTransactionStatus":
                        _globalMoneyLastTransactionStatus = info.InnerText;
                        break;
                    case "GlobalMoneyLastTransactionDate":
                        _globalMoneyLastTransactionDate = DateTime.Parse(info.InnerText);
                        break;
                    case "Sum":
                        _sum = info.InnerText;
                        break;
                    case "DocumentWeight":
                        _documentWeight = info.InnerText;
                        break;
                    case "SumBeforeCheckWeight":
                        _sumBeforeCheckWeight = info.InnerText;
                        break;
                    case "IsEwpPaid":
                        _isEwpPaid = Boolean.Parse(info.InnerText);
                        break;
                    case "IsEwpPaidCashLess":
                        _isEwpPaidCashLess = info.InnerText;
                        break;
                    case "EwPaidSumm":
                        _ewPaidSumm = info.InnerText;
                        break;
                    case "RedeliverySum":
                        _redeliverySum = info.InnerText;
                        break;
                    case "OwnerDocumentType":
                        _ownerDocumentType = info.InnerText;
                        break;
                    case "ChildDocuments":
                        _childDocuments = info.InnerText;
                        break;
                }
            }

        }
    }
}
