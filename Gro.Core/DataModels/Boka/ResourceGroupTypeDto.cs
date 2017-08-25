using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka
{
    public class ResourceGroupTypeDto
    {
        public ResourceGroupTypeDto()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
        }
        [XmlElement("ID")]
        public string Id { get; set; }
        [XmlElement]
        public string Name { get; set; }
    }
}
