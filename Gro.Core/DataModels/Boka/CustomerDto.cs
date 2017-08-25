using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            CustomerNo = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            PhoneNo = string.Empty;
            MobileNo = string.Empty;
            Division = string.Empty;
        }
        [XmlElement("KundNummerID")]
        public string CustomerNo { get; set; }
        [XmlElement("KundNamn")]
        public string Name { get; set; }
        [XmlElement("email")]
        public string Email { get; set; }
        [XmlElement("Telefonnummer_x0020_1")]
        public string PhoneNo { get; set; }
        [XmlElement("telefaxnummer")]
        public string MobileNo { get; set; }
        [XmlElement("Division")]
        public string Division { get; set; }
        public int Status { get; set; }
    }
}
