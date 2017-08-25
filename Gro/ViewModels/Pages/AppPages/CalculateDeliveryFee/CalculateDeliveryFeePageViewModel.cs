using System.Collections.Generic;
using System.Linq;
using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.DeliveryAssuranceDtos;

namespace Gro.ViewModels.Pages.AppPages.CalculateDeliveryFee
{
    public class CalculateDeliveryFeePageViewModel : PageViewModel<CalculateDeliveryFeePage>
    {
        public CalculateDeliveryFeePageViewModel(CalculateDeliveryFeePage currentPage) : base(currentPage)
        {
        }

        public CalculateDeliveryFeePageViewModel(CalculateDeliveryFeePage currentPage, IEnumerable<Item> listLorryTypes, IEnumerable<DeliveryAddress> listDelirAddresses, IEnumerable<Item> listMergedItems) : base(currentPage)
        {
            ListLorryTypesItems = new List<KeyValuePair<string, string>>();
            listLorryTypes = listLorryTypes ?? new List<Item>();
            foreach (var item in listLorryTypes)
            {
                ListLorryTypesItems.Add(new KeyValuePair<string, string>(item.Keyvalue, item.Description));
            }

            ListDeliveryAddressesItems = new List<KeyValuePair<string, string>>();
            listDelirAddresses = listDelirAddresses ?? new List<DeliveryAddress>();
            foreach (var item in listDelirAddresses)
            {
                ListDeliveryAddressesItems.Add(new KeyValuePair<string, string>(item.AddressNumber, $"{item.Street},{item.City}"));
            }

            ListMergedItems = new List<KeyValuePair<string, string>>();
            listMergedItems = listMergedItems ?? new List<Item>();
            foreach (var item in listMergedItems)
            {
                var key = ExtractKeyValueForMergedItem(item.Keyvalue);
                var name = item.IsOpen ? item.Description : $"(A) {item.Description}";
                if (!ListMergedItems.Any(i => i.Key.Equals(key)))
                {
                    ListMergedItems.Add(new KeyValuePair<string, string>(key, name));
                }
            }
        }

        private string ExtractKeyValueForMergedItem(string originalKeyValue)
        {
            if (string.IsNullOrEmpty(originalKeyValue))
            {
                return string.Empty;
            }
            return originalKeyValue.Contains(";") ? originalKeyValue.Split(';')[0] : originalKeyValue;
        }

        public List<KeyValuePair<string, string>> ListLorryTypesItems { get; set; }

        public List<KeyValuePair<string, string>> ListDeliveryAddressesItems { get; set; }

        public List<KeyValuePair<string, string>> ListMergedItems { get; set; }

        public string SupplierId { get; set; }
    }
}