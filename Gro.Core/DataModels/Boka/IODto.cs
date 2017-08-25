using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    public class IODto
    {
        public int Quantity { get; set; }
        public string Warehouse { get; set; }
        public string ItemName { get; set; }
        public string IONumber { get; set; }
        public string Status { get; set; }
        public int LineNumber { get; set; }
        public string WarehouseName { get; set; }
    }
}
