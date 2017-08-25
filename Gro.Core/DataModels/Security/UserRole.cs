using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class UserRole
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember(Name = "roleid")]
        public int Roleid { get; set; }

        [DataMember]
        public bool Sysrole { get; set; }
    }
}
