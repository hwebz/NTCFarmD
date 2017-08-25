using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.ContainerPages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(GUID = "924f4ceb-baf9-4afe-930b-d737c3de656e")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-04.png")]
    [AvailableContentTypes(Exclude = new[] { typeof(FolderPage), typeof(InformationPage) })]
    public class FolderPage : PageData, IContainerPage
    {
    }
}
