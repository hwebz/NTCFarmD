namespace Gro.Core.DataModels.Security
{
    public class OrganizationUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ProfilePicUrl { get; set; }
        public UserRole[] Roles { get; set; }
        public bool LockedOut { get; set; }
        public string RoleProfileId { get; set; }
        public string RoleProfileName { get; set; }
        public int Userid { get; set; }
    }
}
