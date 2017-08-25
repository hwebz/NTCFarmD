using Gro.Core.DataModels.MessageHubDtos;
using DeliveryHeader = Gro.Constants.DeliveryItemHeader;
namespace Gro.Helpers
{
    public static class DeliveryMessageHelper
    {
        public static object MappingMsgInforByName(MessageExtended msg, string header)
        {
            if (header.IsMemberOfList(DeliveryHeader.Ordernr, DeliveryHeader.Ordernumbmer)) return msg.OrderNo;
            if (header.IsMemberOfList(DeliveryHeader.Rad, DeliveryHeader.AntalOrderrader)) return msg.OrderLine;
            if (header.Equals(DeliveryHeader.Artikel)) return msg.ItemName;
            if (header.IsMemberOfList(DeliveryHeader.BestKvantitet, DeliveryHeader.PlaneradKvantitet, DeliveryHeader.BestKvant, DeliveryHeader.SummaKvantitet, DeliveryHeader.Summa)) return msg.OrderQuantity;
            if (header.Equals(DeliveryHeader.Silo)) return msg.Container;
            if (header.IsMemberOfList(DeliveryHeader.Planeradankomst, DeliveryHeader.PlaneradHamtning, DeliveryHeader.PlanAnkomst)) return msg.PlannedDeliveryDate != null ? $"{msg.PlannedDeliveryDate:yyyy-MM-dd}" : string.Empty;
            if (header.IsMemberOfList(DeliveryHeader.FranFabrik, DeliveryHeader.FranLager, DeliveryHeader.LevererasTill, DeliveryHeader.FranFabrikLager, DeliveryHeader.TillLager)) return msg.Warehouse;
            if (header.IsMemberOfList(DeliveryHeader.Levererat, DeliveryHeader.Hamtat)) return msg.DeliveryDate != null ? $"{msg.DeliveryDate:yyyy-MM-dd}" : string.Empty;
            if (header.IsMemberOfList(DeliveryHeader.LevKvant, DeliveryHeader.LevKvantitet)) return msg.DeliveredQuantity;
            if (header.Equals(DeliveryHeader.Enhet)) return msg.Unit;
            if (header.IsMemberOfList(DeliveryHeader.Sandning, DeliveryHeader.Sandnr)) return msg.FreightNo;
            if (header.Equals(DeliveryHeader.Transportor)) return msg.Carrier;
            if (header.Equals(DeliveryHeader.Bil)) return msg.CarNo;
            if (header.Equals(DeliveryHeader.TelTransportor)) return msg.CarMobileNo;
            return header.IsMemberOfList(DeliveryHeader.TelefonBil) ? msg.CarMobileNo : null;
        }

        public static object MappingMsgInforByName(PlannedDelivery msg, string header)
        {
            if (header.IsMemberOfList(DeliveryHeader.FranFabrikLager, DeliveryHeader.TillLager)) return msg.Warehouse;
            if (header.Equals(DeliveryHeader.TelTransportor)) return msg.CarMobileNo;
            if (header.Equals(DeliveryHeader.Transportor)) return msg.Carrier;
            if (header.IsMemberOfList(DeliveryHeader.Sandning, DeliveryHeader.Sandningsnr)) return msg.FreightNo;
            if (header.Equals(DeliveryHeader.AntalOrderrader)) return msg.NoOfRows; //
            if (header.Equals(DeliveryHeader.Artikel)) return msg.ItemName;
            if (header.IsMemberOfList(DeliveryHeader.Summa, DeliveryHeader.SummaKvantitet)) return msg.QuantitySum;
            if (header.Equals(DeliveryHeader.Planeradankomst)) return msg.PlannedDeliveryDate != null ? $"{msg.PlannedDeliveryDate:yyyy-MM-dd}" : string.Empty;

            if (header.Equals(DeliveryHeader.LevForsakran)) return msg.OrderNo;

            return msg.FreightNo;
        }

    }
}