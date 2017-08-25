using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.Security
{
    [DataContract(Name = "ResponseUser", Namespace = "http://schemas.datacontract.org/2004/07/SecurityServiceInt")]
    public class ResponseUser
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsLocketOut { get; set; }

        [DataMember]
        public bool IsSpmlSupplier { get; set; }

        [DataMember]
        public DateTime LastActivityDate { get; set; }

        [DataMember]
        public DateTime LastLockedOutDate { get; set; }

        [DataMember]
        public DateTime LastLoginDate { get; set; }

        [DataMember]
        public string LastLoginFrom { get; set; }

        [DataMember]
        public DateTime LastPasswordChangedDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string OrganisationNo { get; set; }

        [DataMember]
        public string PassWordquestion { get; set; }

        [DataMember]
        public string PhoneMobile { get; set; }

        [DataMember]
        public string PhoneWork { get; set; }

        [DataMember]
        public string PrimaryRole { get; set; }

        [DataMember]
        public string ProviderKey { get; set; }

        [DataMember]
        public string ProviderName { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public int Typ { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Zip { get; set; }

        [DataMember]
        public string ZipAndCity { get; set; }

        [DataMember(Name = "dbUserId")]
        public int DbUserId { get; set; }

        [DataMember(Name = "ProfilePicURL")]
        public string ProfilePicUrl { get; set; }
    }
}
