using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace Gro.Core.ContentTypes.Pages.BasePages
{
    public abstract class AppPageBase : SitePageBase
    {
        [Display(Order = 30, Name = "Body", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Body { get; set; }
    }
}
