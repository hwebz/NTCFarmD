using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.AgreementDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/AgreementServiceInt.Models")]
    public class Agreement
    {
        [DataMember]
        public string AgreementName { get; set; }

        [DataMember]
        public string AgreementNumber { get; set; }

        [DataMember]
        public int AgreementType { get; set; }

        [DataMember]
        public int AntalIntresseanmalan { get; set; }

        [DataMember]
        public string DeliveryTerms { get; set; }

        [DataMember]
        public int HarvestYear { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public DateTime ValidFrom { get; set; }

        [DataMember]
        public DateTime ValidTo { get; set; }

        [DataMember]
        public int Weighed { get; set; }
    }
}
