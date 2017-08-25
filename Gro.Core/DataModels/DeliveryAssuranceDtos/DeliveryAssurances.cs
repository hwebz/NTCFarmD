using System.Runtime.Serialization;

namespace Gro.Core.DataModels.DeliveryAssuranceDtos
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DeliveryAssuranceServiceInt.Models")]
    public class DeliveryAssurances
    {
        [DataMember]
        public DeliveryAssuranceList[] ListOfConfirmed { get; set; }

        [DataMember]
        public DeliveryAssuranceList[] ListOfDelivered { get; set; }

        [DataMember]
        public DeliveryAssuranceList[] ListOfNotConfirmed { get; set; }
    }
}
