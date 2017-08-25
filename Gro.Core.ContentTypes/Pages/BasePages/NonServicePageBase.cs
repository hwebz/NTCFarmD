using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;

namespace Gro.Core.ContentTypes.Pages.BasePages
{
    public abstract class NonServicePageBase : SitePageBase
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10, Name = "Right Navigation Header")]
        public virtual string RightNavigationHeader { get; set; }
    }
}