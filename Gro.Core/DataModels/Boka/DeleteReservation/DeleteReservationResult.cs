using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.DeleteReservation
{
    [XmlRoot]
    public class DeleteReservationResult
    {
       public DeleteResult Result { get; set; }
    }

    public class DeleteResult
    {
        public DeleteResult()
        {
            Error = new List<ErrorMessage>();
        }
        [XmlElement]
        public string ID { get; set; }
        [XmlArray]
        public List<ErrorMessage> Error { get; set; }
    }
}
