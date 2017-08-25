using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class Role
    {
        [DataMember]
        public bool AdminRights { get; set; }

        [DataMember]
        public string RoleDescription { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public bool Sysrole { get; set; }
    }
}
