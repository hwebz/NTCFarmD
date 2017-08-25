using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(GUID = "1BE59F10-64B5-4981-A651-EE4C9BA6C61B")]
    public class DryingAgreementPage : AppPageBase
    {
        [Display(Order = 40, GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString ConfirmationText { get; set; }
    }
}
