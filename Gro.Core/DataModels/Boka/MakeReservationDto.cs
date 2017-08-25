using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    [Serializable]
    public class MakeReservationDto : BaseReservationDto
    {
        public MakeReservationDto()
        {
            Reservations = new List<TimeReservationDto>();
        }

        public string ResourceId { get; set; }
        public string Qty { get; set; }

        public string ContractNo { get; set; }
        public string VehicleAssortmentID { get; set; }
        public string Dried { get; set; }
    
        public string Item { get; set; }
        public string SearchType { get; set; }
        public string SearchValue { get; set; }
        public string SelectedDate { get; set; }
        public string Agrement { get; set; }
        public string CustomerName { get; set; }

        public string CustomerNo { get; set; }
        public List<TimeReservationDto> Reservations { get; set; }
    }
}
