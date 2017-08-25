using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Organization
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/CustomerSupportServiceInt.Models")]
    public class CustomerInvoiceAddress : CustomerBaseAddress
    {
        public string EmailAddress { get; set; }
    }
}
