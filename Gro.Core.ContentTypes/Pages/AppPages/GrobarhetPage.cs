using EPiServer.Core;
using EPiServer.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.AppPages
{
    [ContentType(DisplayName = "Grobarhet", GUID = "a2b6c0de-df9d-4c06-8955-d8b1d3d1f1ee", Description = "")]
    public class GrobarhetPage : AppPageBase
    {
        [Display(Order =200)]
        public virtual string SearchHeader { get; set; }

        [Display(Order = 210)]
        public virtual string SearchDescriptionText { get; set; }

        [Display(Order = 300)]
        public virtual string ExplanationHeader { get; set; }

        [Display(Order = 310)]
        public virtual XhtmlString ExplanationBody { get; set; }
    }
}
