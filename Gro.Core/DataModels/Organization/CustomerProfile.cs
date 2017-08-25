using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "Profile", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public partial class CustomerProfile
    {

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ProfileRow[] Rows { get; set; }
    }
}
