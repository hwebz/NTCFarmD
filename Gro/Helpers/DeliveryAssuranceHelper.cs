using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Gro.Helpers
{
    public static class DeliveryAssuranceHelper
    {
        public static string PreDaysDelivery => ConfigurationManager.AppSettings["PreDaysDelivery"] ?? "0";
        public static string PostDaysDelivery => ConfigurationManager.AppSettings["PostDaysDelivery"] ?? "0";

        private static string HarvestPeriodStart
            => ConfigurationManager.AppSettings["harvestPeriodStart"] ?? string.Empty; //MM-dd
        public static string HarvestPeriodStartDate => $"{DateTime.Now.Year}-{HarvestPeriodStart}"; //yyyy-MM-dd

        private static string HarvestPeriodEnd
           => ConfigurationManager.AppSettings["harvestPeriodEnd"] ?? string.Empty;
        public static string HarvestPeriodEndDate => $"{DateTime.Now.Year}-{HarvestPeriodEnd}";

        private static string StoragePeriodStart
           => ConfigurationManager.AppSettings["storagePeriodStart"] ?? string.Empty;
        private static string StoragePeriodEnd
           => ConfigurationManager.AppSettings["storagePeriodEnd"] ?? string.Empty;
        public static bool IsInHarvestPeriod()
        {
            DateTime result;
            if (!DateTime.TryParse(HarvestPeriodStartDate, out result) ||
                !DateTime.TryParse(HarvestPeriodEndDate, out result))
            return false;

            var harvestStart = Convert.ToDateTime(HarvestPeriodStartDate);
            var harvestEnd = Convert.ToDateTime(HarvestPeriodEndDate);

            return DateTime.Now > harvestStart && DateTime.Now < harvestEnd;
        }
        public static bool IsInStoragePeriod()
        {
            DateTime result;
            if (!DateTime.TryParse(HarvestPeriodStartDate, out result) ||
                !DateTime.TryParse($"{DateTime.Now.Year}-{StoragePeriodStart}", out result) ||
                !DateTime.TryParse($"{DateTime.Now.Year}-{StoragePeriodEnd}", out result))
                return false;

            var harvestStart = Convert.ToDateTime(HarvestPeriodStartDate);
            DateTime storageStart;
            DateTime storageEnd;

            if (DateTime.Now > harvestStart)
            {
                storageStart = Convert.ToDateTime(DateTime.Now.Year + "-" + StoragePeriodStart);
                storageEnd = Convert.ToDateTime((DateTime.Now.Year + 1) + "-" + StoragePeriodEnd);
            }
            else
            {
                storageStart = Convert.ToDateTime((DateTime.Now.Year - 1) + "-" + StoragePeriodStart);
                storageEnd = Convert.ToDateTime(DateTime.Now.Year + "-" + StoragePeriodEnd);
            }

            return DateTime.Now > storageStart && DateTime.Now < storageEnd;
        }

        public static string BuildQueryUrl(string url, Dictionary<string, string> @params)
        {
            var array = (from item in @params
                         where !string.IsNullOrEmpty(item.Value)
                         select $"{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value)}")
                .ToArray();
            return array.Length > 0 ? $"{url}?{string.Join("&", array)}" : url;
        }

        private static string ItemSortSplit => ";-;";

        public static string GetItemSort(string item, string sort) => $"{item}{ItemSortSplit}{sort}";
        public static string ParseItemValue(string val)
        {
            if (val == null)
                return string.Empty;
            var s = Regex.Split(val, ItemSortSplit);
            return s.Any() ? s[0] : string.Empty;
        }

        public static string ParseSortValue(string val)
        {
            if (val == null)
                return string.Empty;
            var s = Regex.Split(val, ItemSortSplit);
            return s.Length >= 1 ? s[1] : string.Empty;
        }


        /// <summary>
        /// Returns empty string if the date contains the minimum value (0001-01-01) else the orginal date as string.
        /// </summary>
        /// <param name="dateToValidate">The date to validate.</param>
        /// <returns></returns>
        public static string GetValidDate(DateTime dateToValidate)
        {
            string retDate;

            // If delivery date is 0001-01-01
            if (dateToValidate == DateTime.MinValue)
            {
                retDate = string.Empty;
            }
            else
            {
                retDate = $"{dateToValidate:yyyy-MM-dd}";

                if (retDate.IndexOf("0001", StringComparison.Ordinal) > 0)
                    retDate = string.Empty;
            }
            return retDate;

        }
    }
}