using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PostWatcher
{
    [Serializable]
    [DataContract]
    public class DataItem
    {

        private string _ref;
        private DateTime _dateTime;
        private DateTime _preferredDeliveryDate;
        private double _weight;
        private byte _seatsAmount;
        private string _intDocNumber;
        private double _cost;
        private string _citySender;
        private string _cityRecipient;
        private string _senderAddress;
        private string _recipientAddress;
        private double _costOnSite;
        private string _payerType;
        private string _paymentMethod;
        private string _afterpaymentOnGoodsCost;
        private string _packingNumber;
        private string _number;
        private string _posted;
        private string _deletionMark;
        private string _cargoType;
        private string _route;
        private string _ewNumber;
        private string _description;
        private string _saturdayDelivery;
        private string _expressWaybill;
        private string _carCall;
        private string _serviceType;
        private DateTime _deliveryDateFrom;
        private string _vip;
        private string _additionalInfomation;
        private DateTime _lastModificationDate;
        private DateTime _receiptDate;
        private string _loyalityCard;
        private string _sender;
        private string _contactSender;
        private string _sendersPhone;
        private string _recipient;
        private string _contactRecipient;
        private string _recipientsPhone;
        private string _redelivery;
        private string _saturdayDeliveryString;
        private string _note;
        private string _thirdPerson;
        private string _forwarding;
        private string _numberOfFloorsLifting;
        private string _statementOfAcceptanceTransferCargoID;
        private string _stateId;
        private string _stateName;
        private string _recipientFullName;
        private string _recipientPost;
        private DateTime _recipientDateTime;
        private string _rejectionReason;
        private string _citySenderDescription;
        private string _cityRecipientDescription;
        private string _senderDescription;
        private string _recipientDescription;
        private string _recipientContactPhone;
        private string _recipientContactPerson;
        private string _senderAddressDescription;
        private string _recipientAddressDescription;
        private bool _printed;
        private string _printedDescription;
        private string _fulfillment;
        private DateTime _estimatedDeliveryDate;
        private DateTime _dateLastUpdatedStatus;
        private DateTime _createTime;
        private string _scanSheetNumber;
        private string _infoRegClientBarcodes;
        private string _statePayId;
        private string _statePayName;
        private string _backwardDeliveryCargoType;

        [DataMember]
        public string Ref
        {
            get { return _ref; }
            private set { _ref = value; }
        }
        [DataMember]
        public DateTime DateTime
        {
            get { return _dateTime; }
            private set { _dateTime = value; }
        }
        [DataMember]
        public DateTime PreferredDeliveryDate
        {
            get { return _preferredDeliveryDate; }
            private set { _preferredDeliveryDate = value; }
        }
        [DataMember]
        public double Weight
        {
            get { return _weight; }
            private set { _weight = value; }
        }
        [DataMember]
        public byte SeatsAmount
        {
            get { return _seatsAmount; }
            private set { _seatsAmount = value; }
        }
        [DataMember]
        public string IntDocNumber
        {
            get { return _intDocNumber; }
            set { _intDocNumber = value; }
        }
        [DataMember]
        public double Cost
        {
            get { return _cost; }
            private set { _cost = value; }
        }
        [DataMember]
        public string CitySender
        {
            get { return _citySender; }
            private set { _citySender = value; }
        }
        [DataMember]
        public string CityRecipient
        {
            get { return _cityRecipient; }
            private set { _cityRecipient = value; }
        }
        [DataMember]
        public string SenderAddress
        {
            get { return _senderAddress; }
            private set { _senderAddress = value; }
        }
        [DataMember]
        public string RecipientAddress
        {
            get { return _recipientAddress; }
            private set { _recipientAddress = value; }
        }
        [DataMember]
        public double CostOnSite
        {
            get { return _costOnSite; }
            private set { _costOnSite = value; }
        }
        [DataMember]
        public string PayerType
        {
            get { return _payerType; }
            private set { _payerType = value; }
        }
        [DataMember]
        public string PaymentMethod
        {
            get { return _paymentMethod; }
            private set { _paymentMethod = value; }
        }
        [DataMember]
        public string AfterpaymentOnGoodsCost
        {
            get { return _afterpaymentOnGoodsCost; }
            private set { _afterpaymentOnGoodsCost = value; }
        }
        [DataMember]
        public string PackingNumber
        {
            get { return _packingNumber; }
            private set { _packingNumber = value; }
        }
        [DataMember]
        public string Posted
        {
            get { return _posted; }
            private set { _posted = value; }
        }
        [DataMember]
        public string Number
        {
            get { return _number; }
            private set { _number = value; }
        }
        [DataMember]
        public string DeletionMark
        {
            get { return _deletionMark; }
            private set { _deletionMark = value; }
        }
        [DataMember]
        public string CargoType
        {
            get { return _cargoType; }
            private set { _cargoType = value; }
        }
        [DataMember]
        public string Route
        {
            get { return _route; }
            private set { _route = value; }
        }
        [DataMember]
        public string EWNumber
        {
            get { return _ewNumber; }
            private set { _ewNumber = value; }
        }
        [DataMember]
        public string Description
        {
            get { return _description; }
            private set { _description = value; }
        }
        [DataMember]
        public string SaturdayDelivery
        {
            get { return _saturdayDelivery; }
            private set { _saturdayDelivery = value; }
        }
        [DataMember]
        public string ExpressWaybill
        {
            get { return _expressWaybill; }
            private set { _expressWaybill = value; }
        }
        [DataMember]
        public string CarCall
        {
            get { return _carCall; }
            private set { _carCall = value; }
        }
        [DataMember]
        public string ServiceType
        {
            get { return _serviceType; }
            private set { _serviceType = value; }
        }
        [DataMember]
        public DateTime DeliveryDateFrom
        {
            get { return _deliveryDateFrom; }
            private set { _deliveryDateFrom = value; }
        }
        [DataMember]
        public string Vip
        {
            get { return _vip; }
            private set { _vip = value; }
        }
        [DataMember]
        public string AdditionalInfomation
        {
            get { return _additionalInfomation; }
            private set { _additionalInfomation = value; }
        }
        [DataMember]
        public DateTime LastModificationDate
        {
            get { return _lastModificationDate; }
            private set { _lastModificationDate = value; }
        }
        [DataMember]
        public DateTime ReceiptDate
        {
            get { return _receiptDate; }
            private set { _receiptDate = value; }
        }

        [DataMember]
        public string LoyalityCard
        {
            get { return _loyalityCard; }
            private set { _loyalityCard = value; }
        }

        [DataMember]
        public string Sender
        {
            get { return _sender; }
            private set { _sender = value; }
        }
        [DataMember]
        public string ContactSender
        {
            get { return _contactSender; }
            private set { _contactSender = value; }
        }
        [DataMember]
        public string SendersPhone
        {
            get { return _sendersPhone; }
            private set { _sendersPhone = value; }
        }
        [DataMember]
        public string Recipient
        {
            get { return _recipient; }
            private set { _recipient = value; }
        }
        [DataMember]
        public string ContactRecipient
        {
            get { return _contactRecipient; }
            private set { _contactRecipient = value; }
        }
        [DataMember]
        public string RecipientsPhone
        {
            get { return _recipientsPhone; }
            private set { _recipientsPhone = value; }
        }
        [DataMember]
        public string Redelivery
        {
            get { return _redelivery; }
            private set { _redelivery = value; }
        }
        [DataMember]
        public string SaturdayDeliveryString
        {
            get { return _saturdayDeliveryString; }
            private set { _saturdayDeliveryString = value; }
        }
        [DataMember]
        public string Note
        {
            get { return _note; }
            private set { _note = value; }
        }
        [DataMember]
        public string ThirdPerson
        {
            get { return _thirdPerson; }
            private set { _thirdPerson = value; }
        }
        [DataMember]
        public string Forwarding
        {
            get { return _forwarding; }
            private set { _forwarding = value; }
        }
        [DataMember]
        public string NumberOfFloorsLifting
        {
            get { return _numberOfFloorsLifting; }
            private set { _numberOfFloorsLifting = value; }
        }
        [DataMember]
        public string StatementOfAcceptanceTransferCargoID
        {
            get { return _statementOfAcceptanceTransferCargoID; }
            private set { _statementOfAcceptanceTransferCargoID = value; }
        }
        [DataMember]
        public string StateId
        {
            get { return _stateId; }
            private set { _stateId = value; }
        }
        [DataMember]
        public string StateName
        {
            get { return _stateName; }
            set { _stateName = value; }
        }
        [DataMember]
        public string RecipientFullName
        {
            get { return _recipientFullName; }
            private set { _recipientFullName = value; }
        }
        [DataMember]
        public string RecipientPost
        {
            get { return _recipientPost; }
            private set { _recipientPost = value; }
        }
        [DataMember]
        public DateTime RecipientDateTime
        {
            get { return _recipientDateTime; }
            private set { _recipientDateTime = value; }
        }
        [DataMember]
        public string RejectionReason
        {
            get { return _rejectionReason; }
            private set { _rejectionReason = value; }
        }
        [DataMember]
        public string CitySenderDescription
        {
            get { return _citySenderDescription; }
            private set { _citySenderDescription = value; }
        }
        [DataMember]
        public string CityRecipientDescription
        {
            get { return _cityRecipientDescription; }
            set { _cityRecipientDescription = value; }
        }
        [DataMember]
        public string SenderDescription
        {
            get { return _senderDescription; }
            private set { _senderDescription = value; }
        }
        [DataMember]
        public string RecipientDescription
        {
            get { return _recipientDescription; }
            private set { _recipientDescription = value; }
        }
        [DataMember]
        public string RecipientContactPhone
        {
            get { return _recipientContactPhone; }
            set { _recipientContactPhone = value; }
        }
        [DataMember]
        public string RecipientContactPerson
        {
            get { return _recipientContactPerson; }
            private set { _recipientContactPerson = value; }
        }
        [DataMember]
        public string SenderAddressDescription
        {
            get { return _senderAddressDescription; }
            private set { _senderAddressDescription = value; }
        }
        [DataMember]
        public string RecipientAddressDescription
        {
            get { return _recipientAddressDescription; }
            private set { _recipientAddressDescription = value; }
        }
        [DataMember]
        public bool Printed
        {
            get { return _printed; }
            private set { _printed = value; }
        }
        [DataMember]
        public string Fulfillment
        {
            get { return _fulfillment; }
            private set { _fulfillment = value; }
        }
        [DataMember]
        public DateTime EstimatedDeliveryDate
        {
            get { return _estimatedDeliveryDate; }
            private set { _estimatedDeliveryDate = value; }
        }
        [DataMember]
        public DateTime DateLastUpdatedStatus
        {
            get { return _dateLastUpdatedStatus; }
            private set { _dateLastUpdatedStatus = value; }
        }
        [DataMember]
        public DateTime CreateTime
        {
            get { return _createTime; }
            private set { _createTime = value; }
        }
        [DataMember]
        public string ScanSheetNumber
        {
            get { return _scanSheetNumber; }
            private set { _scanSheetNumber = value; }
        }
        [DataMember]
        public string InfoRegClientBarcodes
        {
            get { return _infoRegClientBarcodes; }
            private set { _infoRegClientBarcodes = value; }
        }
        [DataMember]
        public string StatePayId
        {
            get { return _statePayId; }
            private set { _statePayId = value; }
        }
        [DataMember]
        public string StatePayName
        {
            get { return _statePayName; }
            private set { _statePayName = value; }
        }
        [DataMember]
        public string BackwardDeliveryCargoType
        {
            get { return _backwardDeliveryCargoType; }
            private set { _backwardDeliveryCargoType = value; }
        }
        [DataMember]
        public string PrintedDescription
        {
            get { return _printedDescription; }
            set { _printedDescription = value; }
        }

        /// <summary>
        /// Initialize all Properties of this instance
        /// </summary>
        /// <param name="xmlDoc">xml Node "Item" of xmlRespose document</param>
        public void LoadXml(XmlNode xmlDoc)
        {
            foreach (XmlNode info in xmlDoc.ChildNodes)
            {
                switch (info.Name)
                {
                    case "Ref":
                        _ref = info.InnerText;
                        break;
                    case "DateTime":
                        DateTime.TryParse(info.InnerText, out _dateTime);
                        break;
                    case "PreferredDeliveryDate":
                        _preferredDeliveryDate = DateTime.Parse(info.InnerText);
                        break;
                    case "Weight":
                        _weight = Double.Parse(info.InnerText);
                        break;
                    case "SeatsAmount":
                        _seatsAmount = Byte.Parse(info.InnerText);
                        break;
                    case "IntDocNumber":
                        _intDocNumber = info.InnerText;
                        break;
                    case "Cost":
                        _cost = Double.Parse(info.InnerText);
                        break;
                    case "CitySender":
                        _citySender = info.InnerText;
                        break;
                    case "CityRecipient":
                        _cityRecipient = info.InnerText;
                        break;
                    case "SenderAddress":
                        _senderAddress = info.InnerText;
                        break;
                    case "RecipientAddress":
                        _recipientAddress = info.InnerText;
                        break;
                    case "CostOnSite":
                        _costOnSite = Double.Parse(info.InnerText);
                        break;
                    case "PayerType":
                        _payerType = info.InnerText;
                        break;
                    case "PaymentMethod":
                        _paymentMethod = info.InnerText;
                        break;
                    case "AfterpaymentOnGoodsCost":
                        _afterpaymentOnGoodsCost = info.InnerText;
                        break;
                    case "PackingNumber":
                        _packingNumber = info.InnerText;
                        break;
                    case "Number":
                        _number = info.InnerText;
                        break;
                    case "Posted":
                        _posted = info.InnerText;
                        break;
                    case "DeletionMark":
                        _deletionMark = info.InnerText;

                        break;
                    case "CargoType":
                        _cargoType = info.InnerText;
                        break;
                    case "Route":
                        _route = info.InnerText;
                        break;
                    case "EWNumber":
                        _ewNumber = info.InnerText;
                        break;
                    case "Description":
                        _description = info.InnerText;
                        break;
                    case "SaturdayDelivery":
                        _saturdayDelivery = info.InnerText;
                        break;
                    case "ExpressWaybill":
                        _expressWaybill = info.InnerText;
                        break;
                    case "CarCall":
                        _carCall = info.InnerText;
                        break;
                    case "ServiceType":
                        _serviceType = info.InnerText;
                        break;
                    case "DeliveryDateFrom":
                        DateTime.TryParse(info.InnerText, out _deliveryDateFrom);
                        break;
                    case "Vip":
                        _vip = info.InnerText;
                        break;
                    case "AdditionalInfomation":
                        _additionalInfomation = info.InnerText;
                        break;
                    case "LastModificationDate":
                        DateTime.TryParse(info.InnerText, out _lastModificationDate);
                        break;
                    case "ReceiptDate":
                        DateTime.TryParse(info.InnerText, out _receiptDate);
                        break;
                    case "LoyalityCard":
                        _loyalityCard = info.InnerText;
                        break;
                    case "Sender":
                        _sender = info.InnerText;
                        break;
                    case "ContactSender":
                        _contactSender = info.InnerText;
                        break;
                    case "SendersPhone":
                        _sendersPhone = info.InnerText;
                        break;
                    case "Recipient":
                        _recipient = info.InnerText;
                        break;
                    case "ContactRecipient":
                        _contactRecipient = info.InnerText;
                        break;
                    case "RecipientsPhone":
                        _recipientsPhone = info.InnerText;
                        break;
                    case "Redelivery":
                        _redelivery = info.InnerText;
                        break;
                    case "SaturdayDeliveryString":
                        _saturdayDeliveryString = info.InnerText;
                        break;
                    case "Note":
                        _note = info.InnerText;
                        break;
                    case "ThirdPerson":
                        _thirdPerson = info.InnerText;
                        break;
                    case "Forwarding":
                        _forwarding = info.InnerText;
                        break;
                    case "NumberOfFloorsLifting":
                        _numberOfFloorsLifting = info.InnerText;
                        break;
                    case "StatementOfAcceptanceTransferCargoID":
                        _statementOfAcceptanceTransferCargoID = info.InnerText;
                        break;
                    case "StateId":
                        _stateId = info.InnerText;
                        break;
                    case "StateName":
                        _stateName = info.InnerText;
                        break;
                    case "RecipientFullName":
                        _recipientFullName = info.InnerText;
                        break;
                    case "RecipientPost":
                        _recipientPost = info.InnerText;
                        break;
                    case "RecipientDateTime":
                        DateTime.TryParse(info.InnerText, out _recipientDateTime);
                        break;
                    case "RejectionReason":
                        _rejectionReason = info.InnerText;
                        break;
                    case "CitySenderDescription":
                        _citySenderDescription = info.InnerText;
                        break;
                    case "CityRecipientDescription":
                        _cityRecipientDescription = info.InnerText;
                        break;
                    case "SenderDescription":
                        _senderDescription = info.InnerText;
                        break;
                    case "RecipientDescription":
                        _recipientDescription = info.InnerText;
                        break;
                    case "RecipientContactPhone":
                        _recipientContactPhone = info.InnerText;
                        break;
                    case "RecipientContactPerson":
                        _recipientContactPerson = info.InnerText;
                        break;
                    case "SenderAddressDescription":
                        _senderAddressDescription = info.InnerText;
                        break;
                    case "RecipientAddressDescription":
                        _recipientAddressDescription = info.InnerText;
                        break;
                    case "Printed":
                        _printed = Int32.Parse(info.InnerText) == 1;
                        break;
                    case "Fulfillment":
                        _fulfillment = info.InnerText;
                        break;
                    case "EstimatedDeliveryDate":
                        DateTime.TryParse(info.InnerText, out _estimatedDeliveryDate);
                        break;
                    case "DateLastUpdatedStatus":
                        DateTime.TryParse(info.InnerText, out _dateLastUpdatedStatus);
                        break;
                    case "CreateTime":
                        DateTime.TryParse(info.InnerText, out _createTime);
                        break;
                    case "ScanSheetNumber":
                        _scanSheetNumber = info.InnerText;
                        break;
                    case "InfoRegClientBarcodes":
                        _infoRegClientBarcodes = info.InnerText;
                        break;
                    case "StatePayId":
                        _statePayId = info.InnerText;
                        break;
                    case "StatePayName":
                        _statePayName = info.InnerText;
                        break;
                    case "BackwardDeliveryCargoType":
                        _backwardDeliveryCargoType = info.InnerText;
                        break;

                }
            }

            _printedDescription = _printed ? "Роздруковано" : "Не роздруковано";
        }
    }
}
