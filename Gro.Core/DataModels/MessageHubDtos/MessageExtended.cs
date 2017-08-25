using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class MessageExtended
    {
        [DataMember]
        public string CarMobileNo { get; set; }

        [DataMember]
        public int Category { get; set; }

        [DataMember]
        public bool DeliveryDatePassed { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string OriginalArtType { get; set; }
        
        [DataMember]
        public string OriginalArtTypeName { get; set; }

        [DataMember]
        public bool PickUp { get; set; }

        [DataMember]
        public string CarNo { get; set; }

        [DataMember]
        public string Carrier { get; set; }

        [DataMember]
        public string Container { get; set; }

        [DataMember]
        public double DeliveredQuantity { get; set; }

        [DataMember]
        public DateTime? DeliveryDate { get; set; }

        [DataMember]
        public string FreightNo { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public int OrderLine { get; set; }

        [DataMember]
        public string OrderNo { get; set; }

        [DataMember]
        public double OrderQuantity { get; set; }

        [DataMember]
        public DateTime? PlannedDeliveryDate { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string Warehouse { get; set; }
    }
}
