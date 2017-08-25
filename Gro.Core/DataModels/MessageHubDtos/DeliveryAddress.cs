using System.Runtime.Serialization;

namespace Gro.Core.DataModels.MessageHubDtos
{
    [DataContract(Name = "DeliveryAddress",
        Namespace = "http://schemas.datacontract.org/2004/07/MessagehubService.Models")]
    public class DeliveryAddress
    {
        [DataMember]
        public string Co { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string MobilePhone { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string ZipCode { get; set; }
    }
}