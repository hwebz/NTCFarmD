using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Gro.Core.DataModels.Boka.DateTimeDto;
using Gro.Core.DataModels.Boka.ResourceGroupTimes;

namespace Gro.Core.DataModels.Boka.ListingBoka
{
    [XmlRoot("ReservationList")]
    public class ResponseSearchBookings
    {
        [XmlElement("Reservation")]
        public List<ReservationSearchBooking> Reservation { get; set; }
    }

    public class ReservationSearchBooking
    {
        [XmlElement("ID")]
        public string Id { get; set; }
        [XmlElement("ResourceGroup")]
        public string ResourceGroup { get; set; }
        [XmlElement("ResourceGroupName")]
        public string ResourceGroupName { get; set; }
        [XmlElement("Resource")]
        public string Resource { get; set; }
        [XmlElement("ResourceName")]
        public string ResourceName { get; set; }
        [XmlElement("From")]
        public FromSearchBooking From { get; set; }
        [XmlElement("To")]
        public ToSearchBooking To { get; set; }
        [XmlElement("SpeditorNo")]
        public string SpeditorNo { get; set; }
        [XmlElement("VehicleAssortmentID")]
        public string VehicleAssortmenId { get; set; }
        [XmlElement("VehicleTypeName")]
        public string VehicleTypeName { get; set; }
        [XmlElement("LicensePlateNo")]
        public string LicensePlateNo { get; set; }
        [XmlElement("ContactTelephone")]
        public string ContactTelephone { get; set; }
        [XmlElement("Leveransforsakransnr")]
        public string Leveransforsakransnr { get; set; }
        [XmlElement("Loading")]
        public string Loading { get; set; }
        [XmlElement("Unloading")]
        public string Unloading { get; set; }
        [XmlElement("CustomerNo")]
        public string CustomerNo { get; set; }
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }
        [XmlElement("Note")]
        public string Note { get; set; }
        [XmlElement("TransportOrderNo")]
        public string TransportOrderNo { get; set; }
        [XmlElement("ContractNo")]
        public string ContractNo { get; set; }
        [XmlElement("MobileNo")]
        public string MobileNo { get; set; }
        [XmlElement("EmailAddress")]
        public string EmailAddress { get; set; }
        [XmlElement("ReminderSMS")]
        public string ReminderSms { get; set; }
        [XmlElement("ReminderEmail")]
        public string ReminderEmail { get; set; }
        [XmlElement("ReminderMinutesBefore")]
        public string ReminderMinutesBefore { get; set; }
        [XmlElement("VerificationSMS")]
        public string VerificationSms { get; set; }
        [XmlElement("VerificationEmail")]
        public string VerificationEmail { get; set; }
        [XmlElement("Owner")]
        public string Owner { get; set; }
        [XmlElement("ReservationItems")]
        public ReservationItemsSearchBooking ReservationItems { get; set; }

        public DateTime? EventStartDate
        {
            get
            {
                
                try
                {
                    var y = Convert.ToInt32(From.Date.Year);
                    var m = Convert.ToInt32(From.Date.Month);
                    var d = Convert.ToInt32(From.Date.Day);

                    var h = Convert.ToInt32(From.Time.Hour);
                    var mm = Convert.ToInt32(From.Time.Minute);
                    return new DateTime(y, m, d, h, mm, 0);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public DateTime? EventEndDate
        {
            get
            {

                try
                {
                    var y = Convert.ToInt32(To.Date.Year);
                    var m = Convert.ToInt32(To.Date.Month);
                    var d = Convert.ToInt32(To.Date.Day);

                    var h = Convert.ToInt32(To.Time.Hour);
                    var mm = Convert.ToInt32(To.Time.Minute);
                    return new DateTime(y, m, d, h, mm, 0);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }

    public class ReservationItemsSearchBooking
    {
        public ReservationItemsSearchBooking()
        {
            ReservationItem = new List<ReservationItem>();
        }
        [XmlElement("ReservationItem")]
        public List<ReservationItem> ReservationItem { get; set; }
    }

    public class FromSearchBooking
    {
        [XmlElement("FromDate")]
        public DateObj Date { get; set; }
        [XmlElement("FromTime")]
        public TimeObj Time { get; set; }
    }

    public class ToSearchBooking
    {
        [XmlElement("ToDate")]
        public DateObj Date { get; set; }
        [XmlElement("ToTime")]
        public TimeObj Time { get; set; }
    }


}
