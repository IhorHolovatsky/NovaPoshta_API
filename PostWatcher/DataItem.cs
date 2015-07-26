using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostWatcher
{
    internal class DataItem
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

        public string Ref
        {
            get { return _ref; }
        }

        public DateTime DateTimee
        {
            get { return _dateTime; }
        }

        public DateTime PreferredDeliveryDate
        {
            get { return _preferredDeliveryDate; }
        }

        public double Weight
        {
            get { return _weight; }
        }

        public int SeatsAmount
        {
            get { return _seatsAmount; }
        }

        public string IntDocNumber
        {
            get { return _intDocNumber; }
        }

        public double Cost
        {
            get { return _cost; }
        }

        public string CitySender
        {
            get { return _citySender; }
        }

        public string CityRecipient
        {
            get { return _cityRecipient; }
        }

        public string SenderAddress
        {
            get { return _senderAddress; }
        }

        public string RecipientAddress
        {
            get { return _recipientAddress; }
        }

        public double CostOnSite
        {
            get { return _costOnSite; }
        }

        public string PayerType
        {
            get { return _payerType; }
        }

        public string PaymentMethod
        {
            get { return _paymentMethod; }
        }

        public string AfterpaymentOnGoodsCost
        {
            get { return _afterpaymentOnGoodsCost; }
        }

        public string PackingNumber
        {
            get { return _packingNumber; }
        }

        public string Posted
        {
            get { return _posted; }
        }

        public string Number
        {
            get { return _number; }
        }

        public string DeletionMark
        {
            get { return _deletionMark; }
        }

        public string CargoType
        {
            get { return _cargoType; }
        }

        public string Route
        {
            get { return _route; }
        }

        public string EWNumber
        {
            get { return _ewNumber; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string SaturdayDelivery
        {
            get { return _saturdayDelivery; }
        }

        public string ExpressWaybill
        {
            get { return _expressWaybill; }
        }

        public string CarCall
        {
            get { return _carCall; }
        }

        public string ServiceType
        {
            get { return _serviceType; }
        }

        public DateTime DeliveryDateFrom
        {
            get { return _deliveryDateFrom; }
        }

        public string Vip
        {
            get { return _vip; }
        }

        public string AdditionalInfomation
        {
            get { return _additionalInfomation; }
        }

        public DateTime LastModificationDate
        {
            get { return _lastModificationDate; }
        }

        public DateTime ReceiptDate
        {
            get { return _receiptDate; }
        }

        public string LoyalityCard
        {
            get { return _loyalityCard; }
        }

        public string Sender
        {
            get { return _sender; }
        }

        public string ContactSender
        {
            get { return _contactSender; }
        }

        public string SendersPhone
        {
            get { return _sendersPhone; }
        }

        public string Recipient
        {
            get { return _recipient; }
        }

        public string ContactRecipient
        {
            get { return _contactRecipient; }
        }

        public string RecipientsPhone
        {
            get { return _recipientsPhone; }
        }

        public string Redelivery
        {
            get { return _redelivery; }
        }

        public string SaturdayDeliveryString
        {
            get { return _saturdayDeliveryString; }
        }

        public string Note
        {
            get { return _note; }
        }

        public string ThirdPerson
        {
            get { return _thirdPerson; }
        }

        public string Forwarding
        {
            get { return _forwarding; }
        }

        public string NumberOfFloorsLifting
        {
            get { return _numberOfFloorsLifting; }
        }

        public string StatementOfAcceptanceTransferCargoID
        {
            get { return _statementOfAcceptanceTransferCargoID; }
        }

        public string StateId
        {
            get { return _stateId; }
        }

        public string StateName
        {
            get { return _stateName; }
        }

        public string RecipientFullName
        {
            get { return _recipientFullName; }
        }

        public string RecipientPost
        {
            get { return _recipientPost; }
        }

        public DateTime RecipientDateTime
        {
            get { return _recipientDateTime; }
        }

        public string RejectionReason
        {
            get { return _rejectionReason; }
        }

        public string CitySenderDescription
        {
            get { return _citySenderDescription; }
        }

        public string CityRecipientDescription
        {
            get { return _cityRecipientDescription; }
        }

        public string SenderDescription
        {
            get { return _senderDescription; }
        }

        public string RecipientDescription
        {
            get { return _recipientDescription; }
        }

        public string RecipientContactPhone
        {
            get { return _recipientContactPhone; }
        }

        public string RecipientContactPerson
        {
            get { return _recipientContactPerson; }
        }

        public string SenderAddressDescription
        {
            get { return _senderAddressDescription; }
        }

        public string RecipientAddressDescription
        {
            get { return _recipientAddressDescription; }
        }

        public bool Printed
        {
            get { return _printed; }
        }

        public string Fulfillment
        {
            get { return _fulfillment; }
        }

        public DateTime EstimatedDeliveryDate
        {
            get { return _estimatedDeliveryDate; }
        }

        public DateTime DateLastUpdatedStatus
        {
            get { return _dateLastUpdatedStatus; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        public string ScanSheetNumber
        {
            get { return _scanSheetNumber; }
        }

        public string InfoRegClientBarcodes
        {
            get { return _infoRegClientBarcodes; }
        }

        public string StatePayId
        {
            get { return _statePayId; }
        }

        public string StatePayName
        {
            get { return _statePayName; }
        }

        public string BackwardDeliveryCargoType
        {
            get { return _backwardDeliveryCargoType; }
        }

        public string PrintedDescription
        {
            get { return _printedDescription; }
          private  set {  _printedDescription = value; }
        }


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
