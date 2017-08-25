using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.ResourceGroupTimes
{
   [XmlRoot]
    public class ResourceGroupTimes
    {
        [XmlArray]
        public List<ResourceGroupTime> Loading { get; set; }
        [XmlArray]
        public List<ResourceGroupTime> Unloading { get; set; }
    }
}
