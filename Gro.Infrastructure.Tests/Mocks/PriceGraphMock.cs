using System;
using System.Collections.Generic;

namespace Gro.Infrastructure.Tests.Mocks
{
    internal static class PriceGraphMock
    {
        private static readonly DateTime today = DateTime.Today;

        public static Dictionary<int, string> GenerateLegends(int number)
        {
            var result = new Dictionary<int, string>(number);
            for (int i = 0; i < number; i++)
            {
                result[i] = $"MockName_{i}";
            }

            return result;
        }

        public static Dictionary<DateTime, double> GenerateRandomValues(int number)
        {
            var result = new Dictionary<DateTime, double>(number);
            var random = new Random();
            for (int i = 0; i < number; i++)
            {
                var key = today.AddDays(-i);
                result[key] = random.Next(0, 10000);
            }

            return result;
        }
    }
}
