using System.Runtime.Serialization;

namespace Gro.Core.DataModels.WeighInDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/WeighInServiceInt.Models")]
    public class WeighInSumAgreementDto
    {
        [DataMember]
        public string Artikelnamn { get; set; }

        [DataMember]
        public string Avtalsnummer { get; set; }

        [DataMember]
        public string Leverantorsnummer { get; set; }

        [DataMember]
        public string Skordear { get; set; }
         
        [DataMember]
        public int Summa { get; set; }
    }
}
