using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Core.Internal;
using System.Text.RegularExpressions;

namespace Gro.Helpers
{
    public static class DataExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            return (from object row in table.Rows select CreateItemFromRow<T>((DataRow)row, properties)).ToList();
        }

        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    property.SetValue(item, row[property.Name], null);
                }
            }
            return item;
        }

        public static double ConvertToTon(this int kg)
        {
            return kg >= 1000 ? (double)kg / 1000 : kg;
        }
        public static int ConvertToKg(this double ton)
        {
            return ton <= 1000 ? (Convert.ToInt32(ton * 1000)) : Convert.ToInt32(ton);
        }

        public static void AddToDictionary<TKey, TElement>(this Dictionary<TKey, TElement> dict, TKey key,
            string element) where TElement : List<string>
        {
            dict = dict ?? new Dictionary<TKey, TElement>();
            if (dict.ContainsKey(key))
            {
                dict[key].Add(element);
            }
            else
            {
                var newElement = new List<string>() { element };
                dict.Add(key, (TElement)newElement);
            }
        }

        public static bool IsMemberOfList(this string str, params string[] strings)
        {
            if (string.IsNullOrEmpty(str) || strings.IsNullOrEmpty()) return false;
            return strings.Any(str.Equals);
        }

        public static string RemoveAllSpaces(this string str)
        {
          return string.IsNullOrWhiteSpace(str)?string.Empty:Regex.Replace(str, @"\s+", string.Empty);
        } 

        public static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }
}
