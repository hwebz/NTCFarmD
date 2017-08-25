using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Gro.Core.ContentTypes.Blocks.BlockAsProperties
{
    [ContentType(GUID = "b9cd8fd9-f4bb-4041-8300-1a3b7f010d86", AvailableInEditMode = false)]
    public class SeoBlock : BlockData
    {
        [Display(Order = 100)]
        public virtual string MetaTitle { get; set; }

        [Display(Order = 200)]
        public virtual string MetaDescription { get; set; }

        [Display(Order = 300)]
        public virtual string MetaKeywords { get; set; }
    }
}