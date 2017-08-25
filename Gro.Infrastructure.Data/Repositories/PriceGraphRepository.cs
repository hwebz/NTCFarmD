using Gro.Core.DataModels.PriceGraph;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.Interceptors.Attributes;
using Gro.Infrastructure.Data.PrisgrafService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Data.Repositories
{
    public class PriceGraphRepository : IPriceGraphRepository
    {
        private struct GraphItem
        {
            public string LegendName;
            public DateTime Date;
            public double Value;
            public int LegendId;
        }

        private struct LegendQueryItem
        {
            public int LegendId { get; set; }
            public string LegendName { get; set; }
            public Dictionary<DateTime, double> LegendData { get; set; }
        }

        private readonly IPrisgrafService _prisgrafService;
        public PriceGraphRepository(IPrisgrafService prisgrafService)
        {
            _prisgrafService = prisgrafService;
        }

        [Cache]
        public async Task<PriceGraphDisplay> GetAllChartDataAsync(DateTime fromDate, DateTime toDate, string ticket)
        {
            var legends = await _prisgrafService.GetChartLegendDataAsync(ticket);
            if (legends == null || legends.Count == 0)
            {
                return new PriceGraphDisplay
                {
                    Data = new List<GraphItemRow>(),
                    Legends = new string[0]
                };
            }

            var legendsSortedById = legends.OrderBy(l => l.Key);
            var legendIdSorted = legendsSortedById.Select(l => l.Key).ToArray();

            var dataQueryTasks = legends.Select(async l =>
            {
                var q = await _prisgrafService.GetChartSeriesDataAsync(fromDate, toDate, l.Key, ticket);
                return new LegendQueryItem
                {
                    LegendId = l.Key,
                    LegendName = l.Value,
                    LegendData = q ?? new Dictionary<DateTime, double>()
                };
            });

            var queryResult = await Task.WhenAll(dataQueryTasks);

            var groupByDate = queryResult
                .SelectMany(q => q.LegendData, (d, item) => new GraphItem
                {
                    Date = item.Key.Date,
                    Value = item.Value,
                    LegendName = d.LegendName,
                    LegendId = d.LegendId
                })
                .GroupBy(b => b.Date);

            var graphRows = new List<GraphItemRow>();
            foreach (var g in groupByDate)
            {
                var itemsOrderedById = g
                    .OrderBy(i => i.LegendId)
                    .ToList();

                var valueGroup = new string[legendIdSorted.Length];

                var itemsPt = 0;
                for (var i = 0; i < legendIdSorted.Length; i++)
                {
                    if (itemsPt >= itemsOrderedById.Count || itemsOrderedById[itemsPt].LegendId != legendIdSorted[i])
                    {
                        valueGroup[i] = string.Empty;
                        continue;
                    }

                    valueGroup[i] = itemsOrderedById[itemsPt].Value.ToString(CultureInfo.CurrentCulture);
                    itemsPt++;
                }

                graphRows.Add(new GraphItemRow
                {
                    Date = g.Key,
                    Values = valueGroup
                });
            }
            return new PriceGraphDisplay
            {
                Legends = legendsSortedById.Select(l => l.Value).ToArray(),
                Data = graphRows
            };

        }

        [Cache]
        public async Task<Dictionary<string, int>> GetSelectablePeriodsAsync(string ticket)
            => await _prisgrafService.GetSelectablePeriodsAsync(ticket) ?? new Dictionary<string, int>();

        [Cache]
        public Task<int> GetStartingPeriodAsync(string ticket) => _prisgrafService.GetStartingPeriodAsync(ticket);
    }
}
