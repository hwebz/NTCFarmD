using System.Collections.Generic;
using Gro.Business;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.DataModels.Security;
using Gro.Helpers;

namespace Gro.ViewModels.Pages.MyProfile
{
    public class MyAccountPageViewModel : PageViewModel<MyAccountPage>
    {
        public MyAccountPageViewModel(MyAccountPage currentPage) : base(currentPage)
        {
        }

        public CustomerBasicInfo CurrentOrganization { get; set; }

        public IEnumerable<GroLinkItem> MyProfileLinks => CurrentPage?.MyProfileLinks != null ? CurrentPage.MyProfileLinks.GetGroLinkItems() : new List<GroLinkItem>();
        public IEnumerable<GroLinkItem> MyCompanyLinks => CurrentPage?.MyCompanyLinks != null ? CurrentPage.MyCompanyLinks.GetGroLinkItems() : new List<GroLinkItem>();
        public IEnumerable<GroLinkItem> UserAndOrgLinks => CurrentPage?.UserAndOrganizationLinks != null ? CurrentPage.UserAndOrganizationLinks.GetGroLinkItems() : new List<GroLinkItem>();

        public string UserProfilePictureUrl { get; set; }
        public string CompanyProfilePictureUrl { get; set; }
    }
}
