using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.LoadResourceGroups
{
    public class Visibility
    {
        [XmlElement("Inside")]
        public bool Inside { get; set; }
        [XmlElement("InsideDeliveryPlanGroup")]
        public bool InsideDeliveryPlanGroup { get; set; }
        [XmlElement("Outside")]
        public bool Outside { get; set; }
    }
}
