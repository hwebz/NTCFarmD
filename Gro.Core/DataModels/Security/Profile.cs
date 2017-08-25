using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "Profile", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class Profile
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Id { get; set; }
    }
}
