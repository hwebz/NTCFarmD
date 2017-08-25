using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "User", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember(Name = "ProfilePicURL")]
        public string ProfilePicUrl { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsLocketOut { get; set; }
    }
}
