using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka.DeleteReservation
{
    [Serializable]
    public class DeleReservationResultDto
    {
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
