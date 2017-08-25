using System.Runtime.Serialization;

namespace Gro.Core.DataModels.ShippingDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FraktServiceInt.Models")]
    public class DeliveryFeeResponse
    {

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public int Distance { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public int Fee { get; set; }

        [DataMember]
        public int Ortsjustering { get; set; }
    }
}
