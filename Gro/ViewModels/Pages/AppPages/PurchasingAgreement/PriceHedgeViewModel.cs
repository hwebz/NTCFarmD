using System;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.PurchasingAgreement
{
    public class PriceHedgeViewModel : PageViewModel<PriceHedgePage>
    {
        public PriceHedgeViewModel(PriceHedgePage currentPage) : base(currentPage)
        {
            SelectedPriceArea = new PriceArea();
            Customer = new Customer();
            PriceHedgeForm = new PriceHedgeFormModel();
        }

        public int PriceLow { get; set; }
        public int CommitQuantityMin { get; set; }
        public int CommitQuantityMax { get; set; }
        public string TimeWithClock { get; set; }
        public PriceArea SelectedPriceArea { get; set; }
        public string AgreementTypeName { get; set; }
        public string AgreementHeading { get; set; }
        public string ProductItemName { get; set; }
        public string DeliveryPeriod { get; set; }
        public string RegisterDate { get; set; }
        public string PurchaseAgreementUrl { get; set; }
        public Customer Customer { get; set; }
        public PriceHedgeFormModel PriceHedgeForm { get; set; }
        public string ConfiramtionText { get; set; }
    }

    public class PriceHedgeFormModel
    {
        public int AgreementId { get; set; }
        public int PriceArea { get; set; }
        public int HarvestYear { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public string DeliveryMode { get; set; }
        public int CommitQuantity { get; set; }
        //public string Warehouse { get; set; }
        //public string Price { get; set; }
        public string ProductItemId { get; set; }
        public string GrainType { get; set; }
        public string PriceType { get; set; }
        public decimal UpperPrice { get; set; }
        public decimal LowerPrice { get; set; }
        public DateTime PriceWatchEndDate { get; set; }
        //public string AgreementTypeCode { get; set; }
        public string AgreementType { get; set; }
        public string TargetAction { get; set; }
        //public string ProductItemName { get; set; }
    }

    public class PurchaseAgreementDates
    {
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public int HarvestYear { get; set; }
    }
}