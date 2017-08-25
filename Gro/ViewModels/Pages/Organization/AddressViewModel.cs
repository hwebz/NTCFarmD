using System.Collections.Generic;
using Castle.Core.Internal;
using Gro.Core.DataModels.Organization;
using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Pages.Organization
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            Silos = new List<SiloItem>();
            Receivers= new List<User>();
            NotificationReceiverModels= new List<NotificationReceiverModel>();
            Notifications= new List<DeliveryReceiver>();
        }

        #region Silos

        public List<SiloItem> Silos { get; set; }

        #endregion

        #region Direction

        //TODO: how to get this direction infor?
        public string Direction { get; set; }

        #endregion

        #region Notification  addresses.

        //TODO: how to get this notifications
        public List<DeliveryReceiver> Notifications { get; set; }

        public List<User> Receivers { get; set; }

        public List<NotificationReceiverModel> NotificationReceiverModels { get; set; }
       
        #endregion

        #region Coordinator

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        #endregion

        #region Adress

        public string AdressStreet { get; set; }
        public string ZipCode { get; set; }
        public string Place { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }

        public string AddressNumber { get; set; }
        #endregion

        #region fields not in the edit form

        public string AddressRow2 { get; set; }
        public string AddressRow4 { get; set; }
        public string Country { get; set; }

        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string FaxNumber { get; set; }
        #endregion
    }

    public class NotificationReceiverModel
    {
        public int UserId { get; set; }
        public string Choosen { get; set; }
    }
}