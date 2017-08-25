using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "ProfileRow", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class ProfileRow
    {
        [DataMember]
        public ProfileField[] Fields { get; set; }

        [DataMember]
        public int RowNr { get; set; }
    }
}
