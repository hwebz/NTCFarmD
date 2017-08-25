using System.Collections.Generic;

namespace Gro.ViewModels.Pages.AppPages.PurchasingAgreement
{
    public class ProductItem
    {
        public string Description { get; set; }
        public bool IsFavourite { get; set; }
        public string GrainCategoryId { get; set; }
        public string GrainType { get; set; }
        public string PriceType { get; set; }
        public List<string> Prices { get; set; }
    }
}