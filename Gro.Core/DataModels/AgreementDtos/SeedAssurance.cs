using System.Runtime.Serialization;

namespace Gro.Core.DataModels.AgreementDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/AgreementServiceInt.Models")]
    public class SeedAssurance
    {
        [DataMember]
        public string AgreementName { get; set; }

        [DataMember]
        public string AgreementNumber { get; set; }

        [DataMember]
        public int AgreementType { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string DeliveryTerms { get; set; }

        [DataMember]
        public int HarvestYear { get; set; }

        [DataMember]
        public string LevTime { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string ProductionSite { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public int WeightIn { get; set; }
    }
}
