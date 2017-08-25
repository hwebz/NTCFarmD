using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    public class ReservationResultDto
    {
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public string ReservationId { get; set; }
        public string StartTime { get; set; }
        public string StartDate { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Message { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceNumber { get; set; }
        public int? PurchaseOrderLine { get; set; }
    }
}
