using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.Core.ContentTypes.Pages.Registration
{
    [ContentType(GUID = "3749E09D-FB0F-4BE7-832E-13A9E97C3840")]
    public class RegisterAccountPage : SitePageBase
    {
        [Display(Name = "Medlemskap i Lantmännen ek. för. ", GroupName = "Membership")]
        public virtual XhtmlString MembershipIntro { get; set; }

        [Display(Name = "Som medlem i Lantmännen är du", GroupName = "Membership")]
        public virtual XhtmlString MembershipDescription { get; set; }

        [Display(Name = "Som medlem i Lantmännen kan du", GroupName = "Membership")]
        public virtual XhtmlString MembershipExplanation { get; set; }

        [Display(Name = "Dina användaruppgifter", GroupName = "Information")]
        public virtual XhtmlString UserInfoText { get; set; }

        [Display(Name = "Betalningsvillkor Lantmännen ek. för och Lantmännen Maskin", GroupName = "Information")]
        public virtual XhtmlString CustomerInfoText { get; set; }

        [Display(Name = "Avtal för e-tjänster", GroupName = "Information")]
        public virtual XhtmlString AttachmentInfoText { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        public virtual ContentArea RightContent { get; set; }
    }
}
