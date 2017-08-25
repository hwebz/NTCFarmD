using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class OrganisationInformation
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string OrganisationId { get; set; }

        [DataMember]
        public string OrganisationName { get; set; }

        [DataMember]
        public string PhoneMobile { get; set; }

        [DataMember]
        public string PhoneWork { get; set; }

        [DataMember]
        public string Zip { get; set; }

    }
}
