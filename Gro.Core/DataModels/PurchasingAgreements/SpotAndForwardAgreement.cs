using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class SpotAndForwardAgreement
    {
        [DataMember]
        public string AgreementId
        {
            get; set;
        }

        [DataMember]
        public string CustomerId
        {
            get; set;
        }

        [DataMember]
        public string CustomerUserName
        {
            get; set;
        }

        [DataMember]
        public string GrainType
        {
            get; set;
        }

        [DataMember]
        public int HarvestYear
        {
            get; set;
        }

        [DataMember]
        public string IPAddress
        {
            get; set;
        }

        [DataMember]
        public string ModeOfDelivery
        {
            get; set;
        }

        [DataMember]
        public decimal Price
        {
            get; set;
        }

        [DataMember]
        public int PriceArea
        {
            get; set;
        }

        [DataMember]
        public string PriceType
        {
            get; set;
        }

        [DataMember]
        public string ProductItemId
        {
            get; set;
        }

        [DataMember]
        public int Quantity
        {
            get; set;
        }

        [DataMember]
        public string UserId
        {
            get; set;
        }

        [DataMember]
        public System.DateTime ValidFrom
        {
            get; set;
        }

        [DataMember]
        public System.DateTime ValidTo
        {
            get; set;
        }

        [DataMember]
        public string WatchAction
        {
            get; set;
        }

        [DataMember]
        public System.DateTime WatchDate
        {
            get; set;
        }


        [DataMember]
        public decimal WatchPriceMinimum
        {
            get; set;
        }
    }
}