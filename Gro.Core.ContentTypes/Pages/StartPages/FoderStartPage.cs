using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Foder Start Page", GUID = "{F75B669E-2AAB-4A11-83FD-820B59C5AF4B}",
        AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage) })]
    public class FoderStartPage : TwoColumnServiceStartPage
    {
    }
}
