using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "PlannedDelivery",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class PlannedDelivery
    {
        [DataMember]
        public string CarMobileNo { get; set; }

        [DataMember]
        public string Carrier { get; set; }

        [DataMember]
        public int Category { get; set; }

        [DataMember]
        public string DeliveryAssurance { get; set; }

        [DataMember]
        public string FreightNo { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public string OrderNo { get; set; }


        [DataMember]
        public int NoOfRows { get; set; }

        [DataMember]
        public bool PickUp { get; set; }

        [DataMember]
        public DateTime? PlannedDeliveryDate { get; set; }

        [DataMember]
        public double QuantitySum { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string Warehouse { get; set; }
    }
}