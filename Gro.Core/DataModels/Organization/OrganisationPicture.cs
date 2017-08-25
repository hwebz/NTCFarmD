using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "OrganisationPicture", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class OrganisationPicture
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PictureId { get; set; }

        [DataMember(Name = "PictureURL")]
        public string PictureUrl { get; set; }
    }
}
