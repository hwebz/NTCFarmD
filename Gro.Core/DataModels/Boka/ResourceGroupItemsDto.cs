using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    public class ResourceGroupItemsDto
    {
        public ResourceGroupItemsDto()
        {
            Items = new List<DropDownDto>();
            ReservationStops = new List<ReservationStopDto>();
            Vehicles = new List<DropDownDto>();
        }
        public List<DropDownDto> Items { get; set; }
        public List<ReservationStopDto> ReservationStops { get; set; }
        public List<DropDownDto> Vehicles { get; set; }
    }
}
