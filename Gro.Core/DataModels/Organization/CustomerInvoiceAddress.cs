using System.Runtime.Serialization;
namespace Gro.Core.DataModels.Organization
{
    [DataContract(Name = "CustomerInvoiceAddress", Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    public class CustomerInvoiceAddress : CustomerBaseAddress
    {
        [DataMember]
        public string EmailAddress { get; set; }
    }
}
