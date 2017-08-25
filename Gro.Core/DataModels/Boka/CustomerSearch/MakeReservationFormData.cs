using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.CustomerSearch
{
    [XmlRoot("MakeReservationFormData")]
    public class MakeReservationFormData
    {
        public MakeReservationFormData()
        {
            Error = new List<ErrorMakeReservation>();
            Item = new List<ItemMakeReservation>();
            FromData = new List<FromDataMakeReservation>();
        }
        [XmlElement("Error")]
        public List<ErrorMakeReservation> Error { get; set; }
        [XmlElement("Item")]
        public List<ItemMakeReservation> Item { get; set; }
        [XmlElement("FromData")]
        public List<FromDataMakeReservation> FromData { get; set; }
    }

    public class ErrorMakeReservation
    {
        [XmlElement("ID")]
        public string Id { get; set; }
        [XmlElement("Message")]
        public string Message { get; set; }
    }

    public class ItemMakeReservation
    {
        [XmlElement("ID")]
        public string Id { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("ItemNo")]
        public string ItemNo { get; set; }
        [XmlElement("Sort")]
        public string Sort { get; set; }
        [XmlElement("Dried")]
        public string Dried { get; set; }
        [XmlElement("UnitName")]
        public string UnitName { get; set; }
        [XmlElement("Loading")]
        public string Loading { get; set; }
        [XmlElement("Unloading")]
        public string Unloading { get; set; }
        [XmlElement("FormData_Id")]
        public string FormDataId { get; set; }
    }

    public class FromDataMakeReservation
    {
        [XmlElement("ResourceGroupID")]
        public string ResourceGroupId { get; set; }
        [XmlElement("CustomerNo")]
        public string CustomerNo { get; set; }
        [XmlElement("Quantity")]
        public string Quantity { get; set; }
        [XmlElement("RegNo")]
        public string RegNo { get; set; }
        [XmlElement("PlannedDate")]
        public string PlannedDate { get; set; }
        [XmlElement("FormData_Id")]
        public string FormDataId { get; set; }
    }
}
