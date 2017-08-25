using System.Runtime.Serialization;

namespace Gro.Core.DataModels.AgreementDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/AgreementServiceInt.Models")]
    public class DryAgreement
    {
        [DataMember]
        public string AgreementExist { get; set; }

        [DataMember]
        public string skordeYear { get; set; }
    }
}