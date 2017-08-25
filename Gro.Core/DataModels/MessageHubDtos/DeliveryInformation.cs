using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "DeliveryInformation",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class DeliveryInformation
    {
        [DataMember]
        public MessageExtended[] FromCustomer { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        [DataMember]
        public MessageExtended[] ToCustomer { get; set; }
    }
}