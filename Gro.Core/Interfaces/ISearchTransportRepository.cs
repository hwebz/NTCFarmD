using Gro.Core.DataModels.SearchTransport;
using System;
using System.Collections.Generic;

namespace Gro.Core.Interfaces
{
    public interface ISearchTransportRepository
    {
        List<ShipmentResponse> SearchByShipmentIdMxExternal(int shipmentIdMx, string currentUsersCustomerNumber);
        List<OrderRowResponse> SearchByCustomerOrderNumberExternal(string customerOrderNumber, string currentUsersCustomerNumber);
        List<ShipmentResponse> SearchByCarrierExternal(string carrier, string currentUsersCustomerNumber, DateTime fromdate, DateTime todate);
        List<ShipmentResponse> SearchByWayBillNumberExternal(string wayBillNumber, string currentUsersCustomerNumber);
        List<OrderRowResponse> SearchByCustomerNumber(string customerNumber, DateTime fromdate, DateTime todate);
        List<OrderRowResponse> SearchByOrderNumberExternal(string orderNumber, string currentUsersCustomerNumber);
        List<ShipmentResponse> GetShipmentsByOrderNumberExt(string orderNumber, string currentUsersCustomerNumber);
        List<OrderRowResponse> GetOrderRowByShipmentId(int shipmentIdMx, string currentUsersCustomerNumber);
        List<ShipmentResponse> GetShipmentsByCustomerOrderNumber(string customerOrderNumber, string currentUsersCustomerNumber);
        Annoncement[] GetAnnoncements(int orderRowId);
    }
}
