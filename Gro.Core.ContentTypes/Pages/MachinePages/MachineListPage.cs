using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Blocks;

namespace Gro.Core.ContentTypes.Pages.MachinePages
{
    [ContentType(DisplayName = "List Machine Page", GUID = "51599b35-c7c1-4677-a2e6-b09fbbe72add", Description = "List Machine Page")]
    public class MachineListPage : SitePageBase 
    {

        [Display(Name = "Machine Detail Page", GroupName = SystemTabNames.Content, Order = 30)]
        [CultureSpecific]
        [AllowedTypes(typeof(MachineDetailPage))]
        public virtual ContentReference MachineDetailPage { get; set; }

        [Display(Order= 40, Name = "Banner Content", GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IOneColumnContainer) })]
        public virtual ContentArea BannerContent { get; set; }

        [Display(Name = "Add Machine Page", GroupName = SystemTabNames.Content, Order = 50)]
        [CultureSpecific]
        [AllowedTypes(typeof(MachineAddPage))]
        public virtual ContentReference AddMachinePage { get; set; }



    }
}