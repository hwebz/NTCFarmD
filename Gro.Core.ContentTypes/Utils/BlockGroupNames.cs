using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;

namespace Gro.Core.ContentTypes.Utils
{
    [GroupDefinitions]
    public static class BlockGroupNames
    {
        #region Constants

        [Display(Order = 10)]
        public const string BlockContent = "Block Content";

        [Display(Order = 20)]
        public const string BlockSetting = "Block Setting";
        #endregion
    }
}