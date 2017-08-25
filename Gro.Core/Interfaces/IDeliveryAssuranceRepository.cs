using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Core.DataModels.DeliveryAssuranceDtos;

namespace Gro.Core.Interfaces
{
    public interface IDeliveryAssuranceRepository
    {
        IEnumerable<Item> GetLorryTypes();
        IEnumerable<DeliveryAddress> GetDeliveryAdresses(string supplier);
        IEnumerable<Item> GetMergedItems(string supplier, DateTime date, bool isExternal);
        DeliveryAssurances GetDeliveryAssurances(string supplier);
        Task<DeliveryAssurances> GetDeliveryAssurancesAsync(string supplier);
        Task<bool> DeleteDeliveryAssuranceAsync(string ioNumber, int lineNumber);
        Task<DeliveryAssurance> GetDeliveryAssuranceAsync(string ioNumber, int lineNumber);
        DeliveryAssurance GetDeliveryAssurance(string ioNumber, int lineNumber);
        Task<DeliveryAssurance> GetDefaultDeliveryAssuranceAsync(string supplier, string deliveryAddress = "");
        Task<Item[]> GetWarehouseListAsync(string item, string sort, DateTime deliverydate);
        string GetWarehouse(string supplier, string addressNumber, string itemNumber, string sort, bool dried, int sludge, bool shortendedStraw);
        Task<string> GetWarehouseSubTypeAsync(string warehouseId);
        Task<Item[]> GetDepaAvtalAsync(string supplier, string item, string sort);
        Task<Item[]> GetDepaAvtalDelAssAsync(string supplier, string item, string sort, DateTime deliveryDate);
        Task<List<KeyValuePair<string, string>>> GetM3ValuesAsync(string radioBtnName);
        Task<string> GetM3DescriptionAsync(string radioBtnName, string value);
        Task<string[]> GetYearsAsync();
        Task<DeliveryAssurance> SaveNewAsync(DeliveryAssurance deliveryAssurance);
        Task<bool> UpdateAsync(DeliveryAssurance deliveryAssurance);
        bool Update(DeliveryAssurance deliveryAssurance);
        Supplier GetSupplier(string number);
    }
}
