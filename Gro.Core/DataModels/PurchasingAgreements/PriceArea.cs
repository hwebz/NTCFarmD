using System.Runtime.Serialization;

namespace Gro.Core.DataModels.PurchasingAgreements
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/PurchasingAgreementServiceInt.Models")]
    public class PriceArea
    {
        [DataMember]
        public bool FavoritePriceAreaId { get; set; }

        [DataMember]
        public string PriceAreaId { get; set; }

        [DataMember]
        public string PriceAreaName { get; set; }
    }
}