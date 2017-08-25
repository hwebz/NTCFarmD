using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Grain
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/GrainService.Models")]
    public class AgreementsDeliverys : AgreementsDeliverysDto
    {
        [DataMember]
        public string AgreementNumber { get; set; }

        [DataMember]
        public int AgreementQuantity { get; set; }

        [DataMember]
        public string ItemName { get; set; }
    }
}
