using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Utils;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;

namespace Gro.Core.ContentTypes.Pages
{
    [ContentType(GUID = "B56F559D-0E01-43DB-BDC9-284E7CA3B0B1")]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-04.png")]
    public class NonServicePageFolder : FolderPage
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10, Name = "Right Navigation Header")]
        public virtual string RightNavigationHeader { get; set; }
    }
}