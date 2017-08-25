using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    public class ReservationStopDto
    {
        public ReservationStopDto()
        {
            Message = string.Empty;
            ResourceName = string.Empty;
            FromDate = string.Empty;
            ToDate = string.Empty;
            FromTime = string.Empty;
            ToTime = string.Empty;
        }

        public string Message { get; set; }
        public string ResourceName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}
