using Gro.Core.DataModels.Boka.ResourceGroupTimes;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka
{
    [XmlRoot]
    public class MakeReservationResult
    {
        public MakeReservationResult()
        {
            ReservationList = new List<Reservation>();
        }
        [XmlArray]
        public List<Reservation> ReservationList { get; set; }
    }
}
