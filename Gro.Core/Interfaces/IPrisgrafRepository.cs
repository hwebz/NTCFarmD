using Gro.Core.DataModels.PriceGraph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IPriceGraphRepository
    {
        Task<int> GetStartingPeriodAsync(string ticket);
        Task<Dictionary<string, int>> GetSelectablePeriodsAsync(string ticket);
        Task<PriceGraphDisplay> GetAllChartDataAsync(DateTime fromDate, DateTime toDate, string ticket);
    }
}
