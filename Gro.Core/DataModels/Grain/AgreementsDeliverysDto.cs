using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Grain
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/GrainService.Models")]
    public class AgreementsDeliverysDto
    {
        [DataMember]
        public int DeliveredQuantity { get; set; }
    }
}
