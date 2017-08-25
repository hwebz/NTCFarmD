using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "DeliveryReceiver", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class DeliveryReceiver 
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }
}
