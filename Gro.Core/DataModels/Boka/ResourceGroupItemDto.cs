using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gro.Core.DataModels.Boka
{
    public class ResourceGroupItemDto
    {
        public ResourceGroupItemDto()
        {
            this.ContractNo = string.Empty;
            this.EmailAdress = string.Empty;
            this.ItemNo = string.Empty;
            this.ItemName = string.Empty;
            this.ToolTip = string.Empty;
            this.ItemID = string.Empty;
            this.Dried = string.Empty;
            this.VehicleTypeName = string.Empty;
            this.TransportOrderNo = string.Empty;
            this.SpeditorNo = string.Empty;
            this.FromDate = string.Empty;
            this.FromDateTime = string.Empty;
            this.ToDate = string.Empty;
            this.ToDateTime = string.Empty;
            this.LicensePlateNo = string.Empty;
            this.CustomerNo = string.Empty;
            this.CustomerName = string.Empty;
            this.Owner = string.Empty;
            this.Qty = string.Empty;
            this.Unit = string.Empty;
            this.ResourceId = string.Empty;
            this.ResourceName = string.Empty;
            this.ReservationId = string.Empty;
            this.Loading = false;
            this.ReminderSMS = false;
            this.Unloading = false;
            this.Leveransforsakransnr = string.Empty;
            this.MobileNo = string.Empty;
            this.Note = string.Empty;
            this.ReminderMinutesBefore = string.Empty;
            this.ReminderEmail = string.Empty;
            this.KontraktArtikel = string.Empty;
            this.SpeditorNo = string.Empty;
            this.IsBooked = false;
            this.UserCanChange = false;
            this.Sort = string.Empty;
        }

        public string FromDate { get; set; }
        public string FromDateTime { get; set; }
        public string ResourceName { get; set; }
        public string ToDate { get; set; }
        public string ToDateTime { get; set; }
        public string LicensePlateNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string SpeditorNo { get; set; }
        public string ContractNo { get; set; }
        public string Owner { get; set; }
        public string Qty { get; set; }
        public string Unit { get; set; }
        public string ResourceId { get; set; }
        public bool ReminderSMS { get; set; }
        public string ReservationId { get; set; }
        public bool Loading { get; set; }
        public bool Unloading { get; set; }
        public string Leveransforsakransnr { get; set; }
        public string MobileNo { get; set; }
        public string Note { get; set; }
        public string ReminderMinutesBefore { get; set; }
        public string ReminderEmail { get; set; }
        public string EmailAdress { get; set; }
        public string KontraktArtikel { get; set; }
        public string TransportOrderNo { get; set; }
        public bool IsBooked { get; set; }
        public bool UserCanChange { get; set; }
        public string VehicleTypeName { get; set; }
        public string Dried { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string ToolTip { get; set; }
        public string Sort { get; set; }

    }
}
