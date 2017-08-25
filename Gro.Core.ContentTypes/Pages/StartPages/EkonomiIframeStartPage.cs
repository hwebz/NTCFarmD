using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Utils;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Ekonomi IFrame Start Page", GUID = "{65e95acd-f90a-48fe-8fd2-fb271e4e8d33}",
        AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage), typeof(EkonomiIFramePage) })]
    public class EkonomiIframeStartPage : ServiceStartPage
    {
        [Display(GroupName = SystemTabNames.Content, Order = 310, Name = "IFrame url")]
        public virtual string IFrameUrl { get; set; }
    }
}
