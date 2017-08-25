using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Gro.Core.ContentTypes.Media
{
    [ContentType(DisplayName = "ImageFile", GUID = "{F4FA3295-C504-4503-BDD1-99CE4C1FE756}", Description = "")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,png,gif,bmp")]
    public class ImageFile : ImageData
    {
        [CultureSpecific]
        [Editable(true)]
        [Display(
            Name = "Title",
            Description = "Title field",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Editable(true)]
        [Display(
            Name = "Description",
            Description = "Description field's description",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual string Description { get; set; }
    }
}