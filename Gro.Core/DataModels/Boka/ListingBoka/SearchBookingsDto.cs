
namespace Gro.Core.DataModels.Boka.ListingBoka
{
    public class SearchBookingsDto
    {
        public string ReservationId { get; set; }
        public string CustomerNo { get; set; }
        public string LicensePlateNo{ get; set; }
        public string CustomerName{ get; set; }
        public string FromTime{ get; set; }
        public string ToTime{ get; set; }
        public string FromDate{ get; set; }
        public string ToDate{ get; set; }
        public string ResourceId{ get; set; }
        public string ResourceName{ get; set; }
        public string ResourceGroupId{ get; set; }
        public string ResourceGroupName{ get; set; }

        public string ToolTip{ get; set; }

        public string SpeditorNo{ get; set; }
        public string VehicleAssortmentId{ get; set; }
        public string VehicleTypeName{ get; set; }
        public string Leveransforsakransnr{ get; set; }
        public bool Loading{ get; set; }
        public bool Unloading{ get; set; }
        public string Owner{ get; set; }
        public bool VerificationEmail{ get; set; }
        public bool VerificationSMS{ get; set; }
        public int ReminderMinutesBefore{ get; set; }
        public bool ReminderEmail{ get; set; }
        public bool ReminderSMS{ get; set; }
        public string EmailAddress{ get; set; }
        public string MobileNo{ get; set; }
        public string ContractNo{ get; set; }
        public string TransportOrderNo{ get; set; }

        public string Note{ get; set; }

        public string ItemId{ get; set; }
        public string ItemName{ get; set; }
        public string ItemNo{ get; set; }
        public bool Dried{ get; set; }
        public string Quantity{ get; set; }
        public string UnitName{ get; set; }
        public bool UserCanChange{ get; set; }
        public string ArrivalDate{ get; set; }
        public string ArrivalTime{ get; set; }
        public string ArrivalTimeDifference{ get; set; }
    }
}
