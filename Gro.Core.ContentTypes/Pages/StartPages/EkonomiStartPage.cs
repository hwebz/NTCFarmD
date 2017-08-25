using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.StartPages
{
    [ContentType(DisplayName = "Ekonomi Start Page", GUID = "{E984A009-DB37-454B-ABE0-18B9170BDBB7}",
        AvailableInEditMode = true)]
    [ImageUrl(IconsFolder.StaticPagesIconsFolder + "CMS-icon-page-02.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(InformationArchivePage), typeof(IFramePage), typeof(ArticlePage), typeof(FolderPage) })]
    public class EkonomiStartPage : ServiceStartPage
    {
        [Display(Order = 20, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) }, RestrictedTypes = new Type[] { })]
        public virtual ContentArea Column1 { get; set; }

        [Display(Order = 21, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) }, RestrictedTypes = new Type[] { })]
        public virtual ContentArea Column2 { get; set; }

        [Display(Order = 23, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) }, RestrictedTypes = new Type[] { })]
        public virtual ContentArea Column3 { get; set; }

        [Display(Order = 24, Name = "Left Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) }, RestrictedTypes = new Type[] { })]
        public virtual ContentArea Column4 { get; set; }
    }
}
