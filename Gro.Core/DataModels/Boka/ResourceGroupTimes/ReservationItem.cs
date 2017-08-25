using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.ResourceGroupTimes
{
    public class ReservationItem
    {
        [XmlElement]
        public string ItemID { get; set; }
        [XmlElement]
        public string ItemName { get; set; }
        [XmlElement]
        public string ItemNo { get; set; }
        [XmlElement]
        public string Sort { get; set; }
        [XmlElement]
        public string Dried { get; set; }
        [XmlElement]
        public float Quantity { get; set; }
        [XmlElement]
        public string UnitName { get; set; }

    }
}
