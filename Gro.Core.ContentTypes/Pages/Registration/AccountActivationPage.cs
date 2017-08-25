using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.ContentTypes.Pages.MyProfile;

namespace Gro.Core.ContentTypes.Pages.Registration
{
    [ContentType(GUID = "B5A67B37-64E9-4618-B793-1E79BD6313C3")]
    public class AccountActivationPage : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        [CultureSpecific]
        public virtual Url ContractApplicationUrl { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        public virtual ContentArea RightContent { get; set; }

        private UserAgreementsPage _userAgreementPage;

        public UserAgreementsPage UserAgreementPageReference => _userAgreementPage ?? (_userAgreementPage = ContentExtensions.GetUserAgreementPage());
    }
}