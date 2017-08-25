using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class PriceProtectStorageAgreement
    {
        [DataMember]
        public string AgreementId { get; set; }

        [DataMember]
        public string CustomerUserName { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string PriceType { get; set; }

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
        public string WatchAction { get; set; }

        [DataMember]
        public DateTime WatchDate { get; set; }

        [DataMember]
        public decimal WatchPrice { get; set; }

        [DataMember]
        public decimal WatchPriceMinimum { get; set; }
    }
}