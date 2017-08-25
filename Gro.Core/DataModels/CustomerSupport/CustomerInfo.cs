namespace Gro.Core.DataModels.CustomerSupport
{
    public class CustomerInfo
    {
        public bool IsActive { get; set; }
        public bool ActiveSpecified { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string Email { get; set; }
        public string OrganizationNumber { get; set; }
        public int UserId { get; set; }
        public bool UserIdSpecified { get; set; }
        public string OwnerName { get; set; }
    }
}
