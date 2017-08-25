using System.Collections.Generic;
using EPiServer.Core;

namespace Gro.ViewModels.Navigation
{
    public class NavigationItemModel
    {
        public ContentReference ContentLink { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<NavigationItemModel> Children { get; set; }

        /// <summary>
        /// using only for Top Navigation
        /// </summary>
        public string CssClass { get; set; }
    }
}
