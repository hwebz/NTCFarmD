using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class Item
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Keyvalue { get; set; }

        public bool IsOpen { get; set; }
    }
}