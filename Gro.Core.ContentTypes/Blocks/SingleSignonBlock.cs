using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Business;
using Gro.Core.ContentTypes.Utils;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;

namespace Gro.Core.ContentTypes.Blocks
{
    [ContentType(DisplayName = "SingleSignonBlock", GUID = "6763e114-ac94-444a-beca-8dc675610894", Description = "SSO block")]
    public class SingleSignonBlock : PushBlock
    {
        [Display(Name = "Non-BankId Link", GroupName = BlockGroupNames.BlockContent, Order = 31)]
        [LinkItemCollectionLimit(Max = 1, Min = 0)]
        public virtual LinkItemCollection NoBankIdLink { get; set; }
    }
}
