using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "PlannedDeliveries",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class PlannedDeliveries
    {
        [DataMember]
        public PlannedDelivery[] FromCustomer { get; set; }

        [DataMember]
        public string InvoiceAddress { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        [DataMember]
        public PlannedDelivery[] ToCustomer { get; set; }

        [DataMember]
        public string DeliveryAddress { get; set; }
    }
}