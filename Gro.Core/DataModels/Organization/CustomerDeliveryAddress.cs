
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "CustomerDeliveryAddress",
        Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    public class CustomerDeliveryAddress : CustomerBaseAddress
    {
        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public SiloItem[] Silos { get; set; }

        [DataMember]
        public string Directions { get; set; }
    }
}
