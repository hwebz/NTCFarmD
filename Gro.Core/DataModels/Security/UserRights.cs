using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "UserRights", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public  class UserRights
    {
        [DataMember]
        public int RightsID { get; set; }

        [DataMember]
        public string RightsName { get; set; }

        [DataMember]
        public int RoleID { get; set; }

        [DataMember]
        public string RoleName { get; set; }
    }
}
