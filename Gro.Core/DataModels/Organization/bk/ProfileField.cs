using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class ProfileField
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
