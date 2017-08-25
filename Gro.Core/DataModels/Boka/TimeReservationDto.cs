using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    [Serializable]
    public class TimeReservationDto
    {
        public string ResourceId { get; set; }
        public string FromTime { get; set; }
        public string Loading { get; set; }
        public string Unloading { get; set; }
    }
}
