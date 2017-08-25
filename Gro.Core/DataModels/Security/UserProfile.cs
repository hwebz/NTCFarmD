using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "UserProfile", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class UserProfile
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Profile { get; set; }

        [DataMember]
        public string ProfileId { get; set; }
    }
}
