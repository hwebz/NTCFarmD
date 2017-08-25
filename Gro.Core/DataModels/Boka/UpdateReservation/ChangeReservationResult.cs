using Gro.Core.DataModels.Boka.DeleteReservation;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.UpdateReservation
{
    public class ChangeReservationResult
    {
        public UpdateResult Result { get; set; }
    }

    public class UpdateResult
    {
        public UpdateResult()
        {
            Error = new List<ErrorMessage>();
        }
        [XmlElement]
        public string ID { get; set; }
        [XmlElement]
        public List<ErrorMessage> Error { get; set; }
    }

}
