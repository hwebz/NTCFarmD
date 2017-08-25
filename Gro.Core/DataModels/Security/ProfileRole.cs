using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "ProfileRole", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class ProfileRole
    {
        [DataMember]
        public string ProfileDescription { get; set; }

        [DataMember]
        public string ProfileHeadline { get; set; }

        [DataMember]
        public string ProfileId { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public string RoleRights { get; set; }
    }
}
