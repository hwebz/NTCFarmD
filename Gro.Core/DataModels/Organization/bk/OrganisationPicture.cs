using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class OrganisationPicture
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PictureURL { get; set; }
    }
}
