using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Organization
{
    [ContentType(DisplayName = "HandleOrganizationPage", GUID = "5635f70f-74c9-4993-a4b2-55525a33a3c1", Description = "")]
    public class HandleOrganizationUserPage : NonServicePageBase
    {
        [AllowedTypes(typeof(ProfileInformationPage))]
        public virtual PageReference ProfileInformationPage { get; set; }
    }
}
