using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.LoadResourceGroups
{
    public class ResourceGroupList
    {
        public ResourceGroupList()
        {
            ResourceGroups = new List<ResourceGroup>();
        }
        [XmlElement("ResourceGroup")]
        public List<ResourceGroup> ResourceGroups { get; set; }
    }
}
