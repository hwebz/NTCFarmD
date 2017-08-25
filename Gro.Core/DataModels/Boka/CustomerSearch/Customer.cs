using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.CustomerSearch
{
    [XmlRoot("Customers")]
    public class CustomerSearch
    {
        public CustomerSearch()
        {
            Customers = new List<CustomerDto>();
        }
        [XmlElement("Customer")]
        public List<CustomerDto> Customers { get; set; }
    }
}
