using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "Customer", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class Company
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerNo { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string OrganisationNo { get; set; }
    }
}
