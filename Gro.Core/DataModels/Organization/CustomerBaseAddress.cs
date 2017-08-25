using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "CustomerBaseAddress", Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    [KnownType(typeof(CustomerInvoiceAddress))]
    [KnownType(typeof(CustomerDeliveryAddress))]
    public class CustomerBaseAddress
    {
        //[DataMember]
        //public string Directions { get; set; }

        [DataMember]
        public string AddressNumber { get; set; }

        [DataMember]
        public string AddressRow1 { get; set; }

        [DataMember]
        public string AddressRow2 { get; set; }

        [DataMember]
        public string AddressRow3 { get; set; }

        [DataMember]
        public string AddressRow4 { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerNumber { get; set; }

        [DataMember]
        public string FaxNumber { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string ZipCode { get; set; }
    }
}
