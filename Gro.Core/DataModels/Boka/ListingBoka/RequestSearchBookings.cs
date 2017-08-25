using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka.ListingBoka
{
    public class RequestSearchBookings
    {
        public string ResourceGroupId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RegNo { get; set; }
        public string CustomerNo { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceNumber { get; set; }
        public int? PurchseOrderLine { get; set; }
    }
}
