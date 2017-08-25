using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class DeliveryAddress
    {
        [DataMember]
        public string AddressNumber { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string NutsCode { get; set; }

        [DataMember]
        public string Street { get; set; }
    }
}
