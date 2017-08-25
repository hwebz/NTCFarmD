using System.Runtime.Serialization;

namespace Gro.Core.DataModels.ShippingDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FraktServiceInt.Models")]
    public class DeliveryFeeRequest
    {
        [DataMember]
        public string DeliveryAddress { get; set; }

        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public string Leveransdatum { get; set; }

        [DataMember]
        public string Levsatt { get; set; }

        [DataMember]
        public decimal Quantity { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public string Suppliernumber { get; set; }
    }
}