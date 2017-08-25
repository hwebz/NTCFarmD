using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka.ResourceGroupTimes
{
    public class RequestGroupTime
    {
        public string ResourceGroup { get; set; }
        public string ItemId { get; set; }
        public string CustomerNo { get; set; }
        public bool Dried { get; set; }
        public decimal Quantity { get; set; }
        public string VehicleTypeId { get; set; }
        public bool _Loading { get; set; }
        public bool _Unloading { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool Loading { get; set; }
        public bool Unloading { get; set; }
    }
}
