using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "ExistingUserRegistration", Namespace = "http://schemas.datacontract.org/2004/07/LM2OrganisationService.Model")]
    public class ExistingUserRegistration
    {
        [DataMember]
        public KeyValuePair<string, byte[]>[] Files { get; set; }

        [DataMember(Name = "User_Email")]
        public string UserEmail { get; set; }

        [DataMember(Name = "User_FirstName")]
        public string UserFirstName { get; set; }

        [DataMember(Name = "User_LastName")]
        public string UserLastName { get; set; }

        [DataMember(Name = "User_Mobile")]
        public string UserMobile { get; set; }

        [DataMember(Name = "User_Phone")]
        public string UserPhone { get; set; }
    }
}
