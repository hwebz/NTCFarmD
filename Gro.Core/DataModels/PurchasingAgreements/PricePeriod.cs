using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public  class PricePeriod
    {
        [DataMember]
        public string AgreementType { get; set; }

        [DataMember]
        public bool FavoriteProductItem { get; set; }

        [DataMember]
        public string GrainCategoryId { get; set; }

        [DataMember]
        public string GrainType { get; set; }

        [DataMember]
        public string PriceAreaId { get; set; }

        [DataMember]
        public string PriceType { get; set; }

        [DataMember]
        public string[] Prices { get; set; }

        [DataMember]
        public string ProductItemDescription { get; set; }

        [DataMember]
        public string ProductItemHierarchy { get; set; }

        [DataMember]
        public string ProductItemId { get; set; }
    }
}