using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class Customer
    {
        [DataMember]
        public string CustPriceAreaId
        {
            get; set;
        }
        [DataMember]
        public string CustomerId
        {
            get; set;
        }

        [DataMember]
        public string CustomerName
        {
            get; set;
        }

        [DataMember]
        public string Email
        {
            get; set;
        }

        [DataMember]
        public string PhoneNumber
        {
            get; set;
        }
    }
}
