using Gro.Core.ContentTypes.Pages.MyProfile;

namespace Gro.ViewModels.Pages.MyProfile
{
    public class HandleSigInInformationViewModel : PageViewModel<HandleSignInInformationPage>
    {
        public HandleSigInInformationViewModel(HandleSignInInformationPage currentPage) : base(currentPage)
        {
        }

        public string SocialSecurityNumber { get; set; }
    }
}
