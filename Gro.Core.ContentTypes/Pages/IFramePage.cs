using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;
using System.ComponentModel;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(GUID = "{5F7750B6-55E9-4EF6-A68D-D8B280474D41}", AvailableInEditMode = true,
        DisplayName = "IFrame Page")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(ArticlePage), typeof(IFramePage)})]
    public class IFramePage : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 310, Name = "IFrame url")]
        public virtual string IFrameUrl { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 320, Name = "IFrame height")]
        public virtual int IFrameHeight { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 330, Name = "IFrame width")]
        public virtual int IFrameWidth { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 330, Name = "Enable navigation")]
        [DefaultValue(true)]
        public virtual bool EnableNavigation { get; set; }
    }
}