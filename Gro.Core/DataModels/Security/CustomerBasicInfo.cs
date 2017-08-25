using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "CustomerList", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt.BaseClasses")]
    public class CustomerBasicInfo
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerNo { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int OwnerUserId { get; set; }

        [DataMember(Name = "ProfilePicURL")]
        public string ProfilePicUrl { get; set; }
    }

    public enum CustomerCheckCode
    {
        SoleTrader = 1,
        LegalEntity = 2,
        CustomerNumberNotExist = -2,
        CustomerNumberNotMatch = -1,
        CustomerNumberActivated = 3,
        Underkund = 4
    }
}
