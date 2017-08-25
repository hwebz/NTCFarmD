using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    [Serializable]
    public class BaseReservationDto
    {
        public string Note { get; set; }

        public string EmailAddress { get; set; }
        public string Leveransforsakransnummer { get; set; }
        public string LicensePlateNo { get; set; }
        public string ReminderInMinutesBefore { get; set; }

        public string MobilePhone { get; set; }
        public int LineNumber { get; set; }
    }
}
