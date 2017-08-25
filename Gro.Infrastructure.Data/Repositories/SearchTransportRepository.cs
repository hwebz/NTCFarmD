using Gro.Core.DataModels.SearchTransport;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.LogiWebService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gro.Infrastructure.Data.Repositories
{
    public class SearchTransportRepository : ISearchTransportRepository
    {
        private readonly TicketProvider _ticketProvider;
        private readonly ILogiWebService _logiWebService;
        public SearchTransportRepository(TicketProvider ticketProvider, ILogiWebService logiWebService)
        {
            _ticketProvider = ticketProvider;
            _logiWebService = logiWebService;
        }



        public List<ShipmentResponse> SearchByShipmentIdMxExternal(int shipmentIdMx, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.SearchByShipmentIdMX(shipmentIdMx, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<ShipmentResponse>();
            if (tempVal != null)
            {
                retVal = tempVal.ToList();
            }
            return retVal;
        }

        public List<OrderRowResponse> GetOrderRowByShipmentId(int shipmentIdMx, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.GetOrderRowsByShipmentIdMX(shipmentIdMx, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<OrderRowResponse>();
            if (tempVal != null)
                retVal = tempVal.ToList();
            return retVal;
        }

        public List<OrderRowResponse> SearchByCustomerOrderNumberExternal(string customerOrderNumber, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.SearchByCustomerOrderNumber(customerOrderNumber, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<OrderRowResponse>();
            if (tempVal != null)
                retVal = tempVal.ToList();
            return retVal;
        }

        public List<ShipmentResponse> GetShipmentsByCustomerOrderNumber(string customerOrderNumber, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.GetShipmentsdByCustomerOrderNumber(customerOrderNumber, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<ShipmentResponse>();
            if (tempVal != null)
            {
                retVal = tempVal.ToList();
            }
            return retVal;
        }

        public List<ShipmentResponse> SearchByCarrierExternal(string carrier, string currentUsersCustomerNumber, DateTime fromdate, DateTime todate)
        {
            var tempVal = _logiWebService.SearchByCarrier(carrier, currentUsersCustomerNumber, fromdate, todate, _ticketProvider.GetTicket());
            var retVal = new List<ShipmentResponse>();
            if (tempVal != null)
                retVal = tempVal.ToList();
            return retVal;
        }

        public List<ShipmentResponse> SearchByWayBillNumberExternal(string wayBillNumber, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.SearchByWaybillNumber(wayBillNumber, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<ShipmentResponse>();
            if (tempVal != null)
                retVal = tempVal.ToList();
            return retVal;
        }

        public List<OrderRowResponse> SearchByCustomerNumber(string customerNumber, DateTime fromdate, DateTime todate)
        {
            var tempVal = _logiWebService.SearchByCustomerNumber(customerNumber, fromdate, todate, _ticketProvider.GetTicket());
            var retVal = new List<OrderRowResponse>();
            if (tempVal != null)
            {
                retVal = tempVal.ToList();
            }
            return retVal;
        }

        public List<ShipmentResponse> GetShipmentsByOrderNumberExt(string orderNumber, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.GetShipmentsdByOrderNumber(orderNumber, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<ShipmentResponse>();
            if (tempVal != null)
            {
                retVal = tempVal.ToList();
            }
            return retVal;
        }

        public List<OrderRowResponse> SearchByOrderNumberExternal(string orderNumber, string currentUsersCustomerNumber)
        {
            var tempVal = _logiWebService.SearchByOrderNumber(orderNumber, currentUsersCustomerNumber, _ticketProvider.GetTicket());
            var retVal = new List<OrderRowResponse>();
            if (tempVal != null)
            {
                retVal = tempVal.ToList();
            }
            return retVal;
        }

        public Annoncement[] GetAnnoncements(int orderRowId)
        {
            var retVal = _logiWebService.GetAnnoncements(orderRowId, _ticketProvider.GetTicket());
            return retVal;
        }
    }
}
