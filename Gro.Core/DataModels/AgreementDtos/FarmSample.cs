using System.Runtime.Serialization;

namespace Gro.Core.DataModels.AgreementDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/AgreementServiceInt.Models")]
    public class FarmSample
    {

        [DataMember]
        public string ProvNr { get; set; } //Leveransadress

        [DataMember]
        public string Analyskod { get; set; }

        [DataMember]
        public string Resultat { get; set; }
       
    }
}
