using Gro.Core.DataModels.Security;

namespace Gro.Business
{
    public class SiteUser : UserCore
    {
        public int AcceptedAgreementVersion { get; set; }
        public string SerialNumber { get; set; }
        public string ActiveCustomerNumber { get; set; }

        public SiteUser()
        {
        }

        public SiteUser(UserCore userCore)
        {
            FirstName = userCore.FirstName;
            LastName = userCore.LastName;
            UserName = userCore.UserName;
            UserId = userCore.UserId;
            ProfilePicUrl = userCore.ProfilePicUrl;
            PersonDn = userCore.PersonDn;
            Email = userCore.Email;
        }
    }
}
