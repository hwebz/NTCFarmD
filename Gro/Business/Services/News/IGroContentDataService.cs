using System.Collections.Generic;
using EPiServer.Core;
using System.Web;
using EPiServer.SpecializedProperties;

namespace Gro.Business.Services.News
{
    public interface IGroContentDataService
    {
        /// <summary>
        /// Find content's closest ancestor of a specific type
        /// </summary>
        /// <typeparam name="TContent"></typeparam>
        /// <param name="content"><see cref="IContent"/> to lookup</param>
        /// <returns>Content's ancestor of type TContent, or null if none exists</returns>
        TContent FindAncestor<TContent>(IContent content) where TContent : PageData;

        /// <summary>
        /// Get the 'siblings' of this page to display for visitors
        /// </summary>
        IEnumerable<TContent> GetSiblingsForVisitor<TContent>(PageData page, HttpContextBase httpContext, bool excludeInvisible = true)
            where TContent : PageData;

        /// <summary>
        /// Get the 'children' of this page to display for visitors
        /// </summary>
        IEnumerable<TContent> GetChildrenForVisitor<TContent>(PageData page, HttpContextBase httpContext, bool excludePageInvisibleInNav = true)
            where TContent : PageData;

        IEnumerable<GroLinkItem> PopulateLinkItems(LinkItemCollection items, HttpContextBase httpContext);
    }
}
