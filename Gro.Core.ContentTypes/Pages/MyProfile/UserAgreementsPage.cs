using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.MyProfile
{
    [ContentType(DisplayName = "User Agreements Page", GUID = "56DFB8BD-6AF1-4D13-9297-408DE0111BFE")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-settings.png")]
    public class UserAgreementsPage : NonServicePageBase, IAccountSettingPage
    {
        [Display(Order = 10, Name = "Term Identity", GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [Editable(false)]
        public virtual string TermId { get; set; }

        [Display(Order = 20, Name = "Version Number", GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [Editable(false)]
        public virtual int Version { get; set; }

        [Display(Order = 20, Name = "Main Body", GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Order = 20, Name = "Instruction Text For Updated Agreement", GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string UpdateInstructionText { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Version = 0;
            TermId = Guid.NewGuid().ToString();
        }
    }
}
