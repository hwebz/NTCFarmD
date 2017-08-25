using Gro.Core.DataModels.PriceGraph;
using Gro.Infrastructure.Data.PrisgrafService;
using Gro.Infrastructure.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static Gro.Infrastructure.Tests.Mocks.PriceGraphMock;

namespace Gro.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class PriceGraphRepositoryTests
    {
        private static readonly string ticket = string.Empty;
        private static readonly DateTime today = DateTime.Today;

        [TestMethod]
        public async Task TestNullLegendList()
        {
            //arrange
            var mock = new Mock<IPrisgrafService>();
            mock.Setup(p => p.GetChartLegendDataAsync(ticket))
                .Returns(Task.FromResult((Dictionary<int, string>)null));
            var service = mock.Object;

            //act
            var repo = new PriceGraphRepository(service);
            var graphDate = await repo.GetAllChartDataAsync(today.AddMonths(-24), today, ticket);

            //assert
            Assert.AreEqual(0, graphDate.Legends.Length);
            Assert.AreEqual(0, graphDate.Data.Count);
        }

        [TestMethod]
        public async Task TestEmptyDataList()
        {
            //arrange
            const int legendsSize = 6;
            var mock = new Mock<IPrisgrafService>();
            mock.Setup(p => p.GetChartLegendDataAsync(ticket))
                .Returns(Task.FromResult(GenerateLegends(legendsSize)));

            for (int i = 0; i < 6; i++)
            {
                mock.Setup(p => p.GetChartSeriesDataAsync(today.AddMonths(-legendsSize), today, i, ticket))
                .Returns(Task.FromResult(GenerateRandomValues(0)));
            }

            //act
            var service = mock.Object;
            var repo = new PriceGraphRepository(service);
            var graphDate = await repo.GetAllChartDataAsync(today.AddMonths(-legendsSize), today, ticket);

            //assert
            Assert.AreEqual(legendsSize, graphDate.Legends.Length);
            Assert.AreEqual(0, graphDate.Data.Count);
        }

        [TestMethod]
        public async Task TestEachDateHasAllValues()
        {
            //arrange
            const int legendsSize = 100;
            var enumerable = Enumerable.Range(0, legendsSize);
            var testDays = enumerable.Select(i => today.AddDays(-i)).ToArray();

            var testValues = enumerable.Select(i => (double)i).ToArray();
            var legends = GenerateLegends(legendsSize);

            var values = new Dictionary<DateTime, double>(legendsSize);
            for (int i = 0; i < legendsSize; i++)
            {
                values[testDays[i]] = testValues[i];
            }

            var mock = new Mock<IPrisgrafService>();
            mock.Setup(p => p.GetChartLegendDataAsync(ticket))
                .Returns(Task.FromResult(legends));
            for (int i = 0; i < legendsSize; i++)
            {
                mock.Setup(p => p.GetChartSeriesDataAsync(today.AddMonths(-legendsSize), today, i, ticket))
                    .Returns(Task.FromResult(values));
            }

            var service = mock.Object;
            var repo = new PriceGraphRepository(service);

            //act
            var graphDate = await repo.GetAllChartDataAsync(today.AddMonths(-legendsSize), today, ticket);

            //Assert the legends array is in correct order
            var legendsSorted = legends.OrderBy(l => l.Key).Select(l => l.Value).ToArray();
            for (int i = 0; i < graphDate.Legends.Length; i++)
            {
                Assert.AreEqual(graphDate.Legends[i], legendsSorted[i]);
            }

            for (int i = 0; i < graphDate.Data.Count; i++)
            {
                Assert.AreEqual(graphDate.Data[i].Date, testDays[i]);
                foreach (var val in graphDate.Data[i].Values)
                {
                    Assert.AreEqual(i.ToString(), val);
                }
            }
        }

        [TestMethod]
        public async Task TestMissingDateValues()
        {
            //arrange
            var legends = GenerateLegends(2);
            var expectedResult = new PriceGraphDisplay
            {
                Legends = legends.OrderBy(l => l.Key).Select(l => l.Value).ToArray(),
                Data = new List<GraphItemRow>
                {
                    new GraphItemRow
                    {
                        Date = today.AddDays(-0),
                        Values = new [] { "0",""}
                    },
                    new GraphItemRow
                    {
                        Date = today.AddDays(-1),
                        Values = new [] { "1",""}
                    },
                    new GraphItemRow
                    {
                        Date = today.AddDays(-2),
                        Values = new [] { "2","2"}
                    }
                }
            };

            var mock = new Mock<IPrisgrafService>();
            mock.Setup(p => p.GetChartLegendDataAsync(ticket))
                .Returns(Task.FromResult(legends));

            mock.Setup(p => p.GetChartSeriesDataAsync(today.AddMonths(-5), today, 0, ticket))
                .Returns(Task.FromResult(new Dictionary<DateTime, double>
                {
                    {today.AddDays(0), 0 },
                    {today.AddDays(-1), 1 },
                    {today.AddDays(-2), 2 },
                }));

            mock.Setup(p => p.GetChartSeriesDataAsync(today.AddMonths(-5), today, 1, ticket))
                .Returns(Task.FromResult(new Dictionary<DateTime, double>
                {
                    {today.AddDays(-2), 2 },
                }));

            var service = mock.Object;
            var repo = new PriceGraphRepository(service);

            //act
            var graphDate = await repo.GetAllChartDataAsync(today.AddMonths(-5), today, ticket);

            //assert
            Assert.AreEqual(graphDate.Legends.Length, 2);
            Assert.AreEqual(graphDate.Data[0].Date, today.AddDays(0));
            Assert.AreEqual(graphDate.Data[1].Date, today.AddDays(-1));
            Assert.AreEqual(graphDate.Data[2].Date, today.AddDays(-2));

            var values0 = string.Join(",", graphDate.Data[0].Values);
            Assert.AreEqual(values0, "0,");
            var values1 = string.Join(",", graphDate.Data[1].Values);
            Assert.AreEqual(values1, "1,");
            var values2 = string.Join(",", graphDate.Data[2].Values);
            Assert.AreEqual(values2, "2,2");
        }

        [TestMethod]
        public async Task GetSelectablePeriodsAsyncTest_ServiceNull()
        {
            //arrange
            var mockService = new Mock<IPrisgrafService>();
            mockService.Setup(w => w.GetSelectablePeriodsAsync("")).Returns(Task.FromResult((Dictionary<string, int>)null));
            var service = mockService.Object;
            var repo = new PriceGraphRepository(service);

            //act
            var dict = await repo.GetSelectablePeriodsAsync("");

            //assert
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public async Task GetSelectablePeriodsAsyncTest_ServiceLive()
        {
            //arrange
            var expected = new Dictionary<string, int>
            {
                {"1",1}
            };
            var mockService = new Mock<IPrisgrafService>();
            mockService.Setup(w => w.GetSelectablePeriodsAsync("")).Returns(Task.FromResult(expected));
            var service = mockService.Object;
            var repo = new PriceGraphRepository(service);

            //act
            var dict = await repo.GetSelectablePeriodsAsync("");

            //assert
            Assert.AreEqual(expected.Count, dict.Count);
            Assert.AreEqual(expected["1"], dict["1"]);
        }
    }
}
