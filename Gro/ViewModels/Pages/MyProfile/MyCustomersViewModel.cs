using System.Collections.Generic;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Pages.MyProfile
{
    public class UserRoleInfoViewModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool HasFullControl { get; set; }

        public bool HasRole { get; set; }
    }

    public class MyCustomersViewModel : PageViewModel<MyCustomersPage>
    {
        public MyCustomersViewModel(MyCustomersPage page) : base(page)
        {
        }

        public CustomerBasicInfo CurrentOrganization { get; set; }

        public IEnumerable<UserRoleInfoViewModel> UserRightsInCustomer { get; set; }
    }
}
