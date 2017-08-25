using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class PriceWatch
    {
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public double AgreedQuantity { get; set; }
        [DataMember]
        public string AgreementType { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string ItemNo { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public string PriceList { get; set; }
        [DataMember]
        public DateTime ValidFrom { get; set; }
        [DataMember]
        public DateTime ValidTo { get; set; }
        [DataMember]
        public DateTime WatchDate { get; set; }
        [DataMember]
        public double WatchPriceMin { get; set; }
    }
}
