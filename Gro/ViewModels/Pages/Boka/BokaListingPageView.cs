using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using Gro.Core.ContentTypes.Pages.BokaPages;
using Gro.Core.DataModels.Boka;

namespace Gro.ViewModels.Pages.Boka
{
    public class BokaListingPageView : PageViewModel<BokaListingPage>
    {
        public BokaListingPageView(BokaListingPage currentPage) : base(currentPage)
        {
            ResourceItemList = new List<ResourceItemDto>();
        }
        public List<ResourceItemDto> ResourceItemList { get; set; }
        public CustomerInfo Customer { get; set; }
    }
}