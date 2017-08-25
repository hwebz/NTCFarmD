using Gro.Core.ContentTypes.Pages.MyProfile;

namespace Gro.ViewModels.Pages.MyProfile
{
    public class UserAgreementsPageViewModel : PageViewModel<UserAgreementsPage>
    {
        public UserAgreementsPageViewModel(UserAgreementsPage currentPage) : base(currentPage)
        {
        }

        public bool HasUpdate { get; set; }
    }
}