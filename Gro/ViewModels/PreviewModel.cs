using System.Collections.Generic;
using EPiServer.Core;
using Gro.Core.ContentTypes.Pages.BasePages;

namespace Gro.ViewModels
{
    public class PreviewModel : PageViewModel<SitePageBase>
    {
        public PreviewModel(SitePageBase currentPage, IContent previewContent) : base(currentPage)
        {
            PreviewContent = previewContent;
            Areas = new List<PreviewArea>();
        }

        public IContent PreviewContent { get; set; }
        public List<PreviewArea> Areas { get; set; }

        public class PreviewArea
        {
            public bool Supported { get; set; }
            public string AreaName { get; set; }
            public string AreaTag { get; set; }
            public ContentArea ContentArea { get; set; }

            public string CssClass { get; set; }
        }
    }
}
