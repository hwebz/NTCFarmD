using Gro.Core.DataModels.Boka.DateTimeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.ResourceGroupTimes
{

    public class Reservation
    {
        [XmlElement]
        public string ID { get; set; }

        public DateTimeObj StartTime { get; set; }

        public DateTimeObj EndTime { get; set; }
        [XmlElement]
        public string SpeditorNo { get; set; }
        [XmlElement]
        public string VehicleAssortmentID { get; set; }
        [XmlElement]
        public string VehicleTypeName { get; set; }
        [XmlElement]
        public string LicensePlateNo { get; set; }
        [XmlElement]
        public string ContactTelephone { get; set; }
        [XmlElement]
        public string Leveransforsakransnr { get; set; }
        [XmlIgnore]
        public bool Loading { get; set; }
        [XmlIgnore]
        public bool LoadingSpecified { get; set; }
        [XmlIgnore]
        public bool Unloading { get; set; }
        [XmlIgnore]
        public bool UnloadingSpecified { get; set; }
        [XmlElement]
        public string CustomerNo { get; set; }
        [XmlElement]
        public string CustomerName { get; set; }
        [XmlElement]
        public string Note { get; set; }
        [XmlElement]
        public string TransportOrderNo { get; set; }
        [XmlElement]
        public string ContractNo { get; set; }
        [XmlElement]
        public string MobileNo { get; set; }
        [XmlElement]
        public string EmailAddress { get; set; }
        [XmlIgnore]
        public bool ReminderSMS { get; set; }
        [XmlIgnore]
        public bool ReminderEmail { get; set; }
        [XmlElement]
        public string ReminderMinutesBefore { get; set; }
        [XmlIgnore]
        public bool VerificationSMS { get; set; }
        [XmlIgnore]
        public bool VerificationEmail { get; set; }
        [XmlElement]
        public string Owner { get; set; }
        [XmlElement]
        public string ReferenceType { get; set; }
        [XmlIgnore]
        public bool ReferenceTypeSpecified { get; set; }
        [XmlElement]
        public string ReferenceNumber { get; set; }
        [XmlElement]
        public string PurchaseOrderLine { get; set; }
        [XmlIgnore]
        public bool PurchaseOrderLineSpecified { get; set; }
        [XmlArray]
        public List<ReservationItem> ReservationItems { get; set; }
        [XmlElement]
        public string Error { get; set; }
        [XmlElement]
        public string ChangedBy { get; set; }
        [XmlElement]
        public string Resource { get; set; }
        [XmlElement]
        public string IPAddress { get; set; }
    }
}

enum ReferenceType
{
    PurchaseOrder = 0,
    CustomerOrder = 1,
    DistributionOrder = 2,
    Shipment = 3
};
