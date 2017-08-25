using Gro.Core.DataModels.Security;

namespace Gro.ViewModels.Organization
{
    public class RoleProfileViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public ProfileRole[] ProfileRoles { get; set; }
    }
}
