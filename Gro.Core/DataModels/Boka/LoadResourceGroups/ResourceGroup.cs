using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.LoadResourceGroups
{
    public class ResourceGroup
    {
        public ResourceGroup()
        {
            ResourceGroupType = new ResourceGroupTypeDto();
            Visibility = new Visibility();
        }
        [XmlElement("ID")]
        public string Id { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public bool RegNoMandatory { get; set; }
        [XmlElement]
        public string StorageAreaNo { get; set; }
        [XmlElement]
        public bool HasLoading { get; set; }
        [XmlElement]
        public bool HasUnloading { get; set; }
        [XmlElement("ResourceGroupType")]
        public ResourceGroupTypeDto ResourceGroupType { get; set; }
        [XmlElement]
        public string WebPage { get; set; }
        [XmlElement("M3ID")]
        public string M3Id { get; set; }
        [XmlElement]
        public bool AllowManualReservations { get; set; }
        [XmlElement("Visibility")]
        public Visibility Visibility { get; set; }
    }
}
