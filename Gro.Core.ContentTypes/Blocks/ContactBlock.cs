using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Gro.Core.ContentTypes.Blocks.BlockWidths;
using Gro.Core.ContentTypes.Blocks.BootstrapContainers;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "Contact Block", GUID = "465f042a-9061-4800-9e25-cd73fdb7f928", Description = "Contact Block")]
    public class ContactBlock : BlockData, IOneColumnContainer, IOneColumnWidth
    {
        [Display(GroupName = BlockGroupNames.BlockContent, Order = 10)]
        [CultureSpecific]
        [Required]
        public virtual string Heading { get; set; }

        [Display(GroupName = BlockGroupNames.BlockContent, Order = 20)]
        [CultureSpecific]
        public virtual XhtmlString Description { get; set; }

        [Display(GroupName = BlockGroupNames.BlockContent, Order = 30)]
        [CultureSpecific]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "/common/validations/contactblock/telephonenumberinvalid")]
        public virtual string TelephoneNumber { get; set; }

        [Display(GroupName = BlockGroupNames.BlockContent, Order = 40)]
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        public virtual string OpeningHours { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 10)]
        [CultureSpecific]
        public virtual string LinkToContactPageText { get; set; }

        [Display(GroupName = BlockGroupNames.BlockSetting, Order = 20)]
        [CultureSpecific]
        public virtual ContentReference LinkToContactPage { get; set; }
    }
}
