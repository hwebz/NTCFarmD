using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.ViewModels.Users;

namespace Gro.ViewModels.Pages.MyProfile
{
    public class ProfilePageViewModel : PageViewModel<ProfilePage>
    {
        public EditProfileViewModel ProfileViewModel { get; set; }

        public ProfilePageViewModel(ProfilePage page, EditProfileViewModel profileViewModel):base(page)
        {
            ProfileViewModel = profileViewModel;
        }
    }
}
