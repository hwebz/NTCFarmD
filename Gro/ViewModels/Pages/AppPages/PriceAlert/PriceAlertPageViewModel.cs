using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.PriceAlert
{
    public class PriceAlertPageViewModel : PageViewModel<PriceAlertPage>
    {
        public PriceAlertPageViewModel(PriceAlertPage page) : base(page)
        {
            PriceWatchList = new List<PriceWatchView>();
        }

        public List<PriceWatchView> PriceWatchList { get; set; }
    }

    public class PriceWatchView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double PriceMin { get; set; }
        public string DeliveryPeriod { get; set; }
        public string ContractType { get; set; }
        public string ValidTo { get; set; }
        public string Activity { get; set; }
    }
}