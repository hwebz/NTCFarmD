using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Name = "Product", Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class Product
    {
        [DataMember]
        public string GrainName { get; set; }

        [DataMember]
        public string ProductItemId { get; set; }

        [DataMember]
        public string ProductItemName { get; set; }
    }
}