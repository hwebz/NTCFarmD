using System;
using System.Runtime.Serialization;

namespace Gro.Core.DataModels.CustomerSupport
{
    [DataContract(Name = "Customer",
        Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    public class Customer
    {
        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string Address3 { get; set; }

        [DataMember]
        public string Address4 { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public string CustomerSegment { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string MobilePhone { get; set; }

        [DataMember]
        public string OrganizationNumber { get; set; }

        [DataMember]
        public string Phone1 { get; set; }

        [DataMember]
        public string Phone2 { get; set; }

        [DataMember]
        public string[] Retailers { get; set; }

        [DataMember]
        public string VatNumber { get; set; }
    }
}