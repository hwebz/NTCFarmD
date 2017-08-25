using System;
using System.Xml.Serialization;
using Gro.Core.DataModels.Boka.DateTimeDto;

namespace Gro.Core.DataModels.Boka.ResourceGroupTimes
{
    [XmlRoot("ResourceGroupTime")]
    public class ResourceGroupTime
    {
        [XmlElement]
        public string Resource { get; set; }
        [XmlElement]
        public string ResourceName { get; set; }
        [XmlElement]
        public DateTimeObj DateFrom { get; set; }
        [XmlElement]
        public DateTimeObj DateTo { get; set; }
        public Reservation Reservation { get; set; }

    }
}
