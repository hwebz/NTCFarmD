using System.Collections.Generic;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.PurchasingAgreement
{
    public class ProductItemCategory
    {
        public string ID { get; set; }
        public string Hierarchy { get; set; }
        public List<PricePeriod> ProductItems { get; set; }
    }
}