using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class Supplier
    {
        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string SupplierNumber { get; set; }
    }
}