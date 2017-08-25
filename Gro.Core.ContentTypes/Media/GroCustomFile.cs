using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Gro.Core.ContentTypes.Media
{
    [ContentType(DisplayName = "FarmdayCustomFile", GUID = "9de87cd4-dbda-4470-b018-d5fe8a4011fa", Description = "")]
    [MediaDescriptor(ExtensionString = "pdf,doc,docx,xlsx,xls")]
    public class GroFile : MediaData
    {

        [CultureSpecific]
        [Editable(true)]
        [Display(
            Name = "Description",
            Description = "File description",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Description { get; set; }
    }
}
