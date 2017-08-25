using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class DryingAgreement
    {
        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public string DeliveryTerms
        {
            get; set;
        }

        [DataMember]
        public string Description
        {
            get; set;
        }

        [DataMember]
        public int HarvestYear
        {
            get; set;
        }

        [DataMember]
        public string HeaderText
        {
            get; set;
        }

        [DataMember]
        public string MaxMoisture
        {
            get; set;
        }

        [DataMember]
        public string OtherTerms
        {
            get; set;
        }

        [DataMember]
        public string Price
        {
            get; set;
        }

        [DataMember]
        public string PriceTerms
        {
            get; set;
        }

        [DataMember]
        public string QuantityTerms
        {
            get; set;
        }

        [DataMember]
        public string TermsAttachments
        {
            get; set;
        }

        [DataMember]
        public string Text
        {
            get; set;
        }
    }
}