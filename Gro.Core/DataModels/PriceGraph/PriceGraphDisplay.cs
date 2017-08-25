using System;
using System.Collections.Generic;

namespace Gro.Core.DataModels.PriceGraph
{
    public class PriceGraphDisplay
    {
        public string[] Legends { get; set; }
        public IList<GraphItemRow> Data { get; set; }
    }

    public class GraphItemRow
    {
        public DateTime Date;
        public string[] Values;
    }
}
