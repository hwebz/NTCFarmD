using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class PurchaseAgreement
    {
        [DataMember]
        public string AgreementId { get; set; }

        [DataMember]
        public string ProductItemId
        {
            get; set;
        }

        [DataMember]
        public string Quantity
        {
            get; set;
        }

        [DataMember]
        public string ValidFrom
        {
            get; set;
        }

        [DataMember]
        public string ValidUntil
        {
            get; set;
        }
    }
}