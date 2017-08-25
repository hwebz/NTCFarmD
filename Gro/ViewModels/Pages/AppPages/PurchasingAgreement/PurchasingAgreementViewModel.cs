using System;
using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.AppPages.PurchasingAgreement;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.PurchasingAgreement
{
    public class PurchasingAgreementViewModel : PageViewModel<PurchasingAgreementPage>
    {
        public PurchasingAgreementViewModel(PurchasingAgreementPage currentPage) : base(currentPage)
        {
            StorageAgreements = new List<StorageAgreement>();
            Periods = new Dictionary<string, string>();
            FormModel = new PurchasingAgreementFormModel();
            Products = new Product[0];
            GrainTypes = new Product[0];
            SelectedPriceArea = new PriceArea();
        }
        public List<StorageAgreement> StorageAgreements { get; set; }
        public StorageAgreement SelectedAgreement { get; set; }
        public Dictionary<string, string> Periods { get; set; }
        public int ReferencePrice { get; set; }
        public PurchasingAgreementFormModel FormModel { get; set; }

        public Product[] Products { get; set; }
        public Product[] GrainTypes { get; set; }

        //TODO: update later, there is no spec for this min price
        public int MinPrice { get; set; }
        public int CommitQuantityMin { get; set; }
        public Dictionary<int, string> ModesOfDelivery { get; set; }
        public string Header { get; set; }
        public KeyValuePair<string,string> DepaPeriod { get; set; }
        public PriceArea SelectedPriceArea { get; set; }
    }

    public class PurchasingAgreementFormModel
    {
        public int AgreementId { get; set; }
        public string ProductItemId { get; set; }
        public string GrainType { get; set; }
        public string TargetAction { get; set; }
        public decimal UpperPrice { get; set; }
        public decimal LowerPrice { get; set; }
        public DateTime PriceWatchEndDate { get; set; }
        public string PriceType { get; set; }
        public string DeliveryMode { get; set; }
        public int CommitQuantity { get; set; }
        public int PriceArea { get; set; }
        public string AgreementPeriod { get; set; }
    }

    public class PurchaseAgreementPeriod
    {
        public string PriceType { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public int HarvestYear { get; set; }
    }
}