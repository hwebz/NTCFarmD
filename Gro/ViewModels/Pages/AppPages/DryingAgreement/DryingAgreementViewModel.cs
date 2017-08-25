using Gro.Core.ContentTypes.Pages.AppPages;
using Gro.Core.DataModels.PurchasingAgreements;

namespace Gro.ViewModels.Pages.AppPages.DryingAgreement
{
    public class DryingAgreementViewModel : PageViewModel<DryingAgreementPage>
    {
        public DryingAgreementViewModel(DryingAgreementPage currentPage) : base(currentPage)
        {
            Agreement = new Core.DataModels.PurchasingAgreements.DryingAgreement();
            Customer = new Customer();
        }
        
        public Core.DataModels.PurchasingAgreements.DryingAgreement Agreement { get; set; }
        public Customer Customer { get; set; }

    }
}