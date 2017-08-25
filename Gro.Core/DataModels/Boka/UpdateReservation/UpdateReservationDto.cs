using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka.UpdateReservation
{
    public class UpdateReservationDto : BaseReservationDto
    {
        public string ReservationId { get; set; }
        public string Owner { get; set; }
        public string SpeditorNo { get; set; }
        public string OldCustomerName { get; set; }
        public string OldCustomerNo { get; set; }
        public string NewCustomerName { get; set; }
        public string NewCustomerNo { get; set; }
        public string CurrentCustomerName { get; set; }
        public string CurrentCustomerNo { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
