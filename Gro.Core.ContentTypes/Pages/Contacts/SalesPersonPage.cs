using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Gro.Core.ContentTypes.Business;
using Gro.Core.ContentTypes.Business.UIDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Pages.Contacts
{
    [ContentType(DisplayName = "SalesPersonPage", GUID = "43b270af-b370-4e34-9d3f-981ea8496276", Description = "")]
    public class SalesPersonPage : ArticlePage
    {
        [Required]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<ServiceTeam>))]
        public virtual ServiceTeam TeamCategory { get; set; }
    }
}
