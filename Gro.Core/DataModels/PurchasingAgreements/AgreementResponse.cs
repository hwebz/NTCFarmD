using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class AgreementResponse 
    {
        [DataMember]
        public string AgreementNumber
        {
            get; set;
        }

        [DataMember]
        public System.Nullable<int> Id { get; set; }

        [DataMember]
        public string Message
        {
            get; set;
        }
    }
}