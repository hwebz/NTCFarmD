using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gro.Core.DataModels.DeliveryAssuranceDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.DeliveryAssuranceService;
using Gro.Infrastructure.Data.Interceptors.Attributes;

namespace Gro.Infrastructure.Data.Repositories
{
    public class DeliveryAssuranceRepository : IDeliveryAssuranceRepository
    {
        private readonly IDeliveryAssuranceService _service;
        private readonly TicketProvider _ticketProvider;

        private string Ticket => _ticketProvider.GetTicket();
        public DeliveryAssuranceRepository(IDeliveryAssuranceService deliveryAssuranceService, TicketProvider ticketProvider)
        {
            _service = deliveryAssuranceService;
            _ticketProvider = ticketProvider;
        }

        #region Methods are used in Calculate Delivery Fee Page

        [Cache]
        public IEnumerable<Item> GetLorryTypes() => _service.GetLorryTypes(Ticket);

        [Cache]
        public IEnumerable<DeliveryAddress> GetDeliveryAdresses(string supplier)
            => _service.GetDeliveryAddresses(supplier, Ticket);

        /// <summary>
        /// GetMergedItems
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="date"></param>
        /// <param name="isExternal">true if request comming from external source, false if from internal.</param>
        /// <returns></returns>
        public IEnumerable<Item> GetMergedItems(string supplier, DateTime date, bool isExternal)
        {
            var listMergedItems = _service.GetAgreementItemsDate(supplier, date, Ticket).ToList();
            var listOpenedItems = _service.GetAllOpenItems(date, isExternal, Ticket);
            if (listOpenedItems != null && listOpenedItems.Any())
            {
                listMergedItems.AddRange(listOpenedItems.Select(item => new Item()
                {
                    IsOpen = true,
                    Keyvalue = item.Keyvalue,
                    Description = item.Description
                }));
            }
            return listMergedItems;
        }
        #endregion

        #region Methods are used in Delivery Assurance Page

        public DeliveryAssurances GetDeliveryAssurances(string supplier) => _service.GetDeliveryAssurances(supplier, Ticket);
        public async Task<DeliveryAssurances> GetDeliveryAssurancesAsync(string supplier) => await _service.GetDeliveryAssurancesAsync(supplier, Ticket);
        public async Task<bool> DeleteDeliveryAssuranceAsync(string ioNumber, int lineNumber) => await _service.DeleteIOAsync(ioNumber, lineNumber, Ticket);
        public async Task<DeliveryAssurance> GetDeliveryAssuranceAsync(string ioNumber, int lineNumber) => await _service.GetDeliveryAssuranceAsync(ioNumber, lineNumber, Ticket);
        public DeliveryAssurance GetDeliveryAssurance(string ioNumber, int lineNumber) => _service.GetDeliveryAssurance(ioNumber, lineNumber,Ticket);

        public async Task<DeliveryAssurance> GetDefaultDeliveryAssuranceAsync(string supplier,string deliveryAddress="") => await _service.GetDefaultvaluesAsync(supplier, deliveryAddress, Ticket);
        public async Task<Item[]> GetWarehouseListAsync(string item, string sort, DateTime deliverydate) => await _service.GetWarehouseListAsync(item, sort, deliverydate, Ticket);

        public string  GetWarehouse(string supplier, string addressNumber, string itemNumber, string sort, bool dried, int sludge,
            bool shortendedStraw) => _service.GetWareHouses(sort, addressNumber, supplier, dried, itemNumber, sludge, shortendedStraw,
                Ticket);

        public async Task<string> GetWarehouseSubTypeAsync(string warehouseId)
            => await _service.GetWarehouseSubtypeAsync(warehouseId, Ticket);

        public async Task<Item[]> GetDepaAvtalAsync(string supplier, string item, string sort)
            => await _service.GetDepaAvtalAsync(supplier, item, sort, Ticket);
        public async Task<Item[]> GetDepaAvtalDelAssAsync(string supplier, string item, string sort, DateTime deliveryDate)
            => await _service.GetDepaAvtalDelAssAsync(supplier, item, sort, deliveryDate, Ticket);

        public async Task<List<KeyValuePair<string, string>>> GetM3ValuesAsync(string radioBtnName)
        {
            var m3Value = await _service.GetM3ValuesForRadioButtonAsync(radioBtnName,Ticket);
            var retVal = GetKeyValuePairFromString(m3Value);
            return retVal;
        }

        public async Task<string> GetM3DescriptionAsync(string radioBtnName, string value)
        {
            var retVal = await GetM3ValuesAsync(radioBtnName);
            return GetDescription(value, retVal);
        }

        [Cache]
        public async Task<string[]> GetYearsAsync() => await _service.GetYearsAsync(Ticket);

        public async Task<DeliveryAssurance> SaveNewAsync(DeliveryAssurance deliveryAssurance) => await _service.SaveNewAsync(deliveryAssurance, Ticket);

        public async Task<bool> UpdateAsync(DeliveryAssurance deliveryAssurance) => await _service.UpdateAsync(deliveryAssurance, Ticket);
        public bool Update(DeliveryAssurance deliveryAssurance) => _service.Update(deliveryAssurance, Ticket);

        public Supplier GetSupplier(string number) => _service.GetSupplier(number,Ticket);

        /// <summary>
        /// Gets the description for a specific value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <returns></returns>
        private static string GetDescription(string value, IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var itemAndDescription = keyValuePairs.FirstOrDefault(i => i.Key == value);
            return !string.IsNullOrWhiteSpace(itemAndDescription.Value) ? itemAndDescription.Value : string.Empty;
        }

        /// <summary>
        /// Gets the key value pair from string.
        /// </summary>
        /// <param name="m3Value">The m3 value.</param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> GetKeyValuePairFromString(string m3Value)
        {
            var retVal = new List<KeyValuePair<string, string>>();
            var valueAndDescriptions = Regex.Split(m3Value, @"\|");

            foreach (var item in valueAndDescriptions)
            {
                var valueAndDescription = item.Split(new[] { ":" }, StringSplitOptions.None);
                if (valueAndDescription.Count() >= 2)
                {
                    retVal.Add(new KeyValuePair<string, string>(valueAndDescription[0], valueAndDescription[1]));
                }
            }
            return retVal;
        }
        #endregion
    }
}
