using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.Registration
{
    [ContentType(GUID = "bb3dd20a-b961-437d-ad5e-8b78ddf77828")]
    public class RegistrationHomePage : SitePageBase
    {
        [Display(Name = "Account registration page", Order = 10)]
        [CultureSpecific]
        [AllowedTypes(typeof(RegisterAccountPage))]
        public virtual ContentReference RegisterAccountPage { get; set; }

        [Display(Name = "Activate account page", Order = 20)]
        [CultureSpecific]
        [AllowedTypes(typeof(AccountActivationPage))]
        public virtual ContentReference ActivateCustomerPage { get; set; }
    }
}
