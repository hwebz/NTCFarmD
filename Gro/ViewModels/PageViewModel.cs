using EPiServer.Core;
using Gro.Business;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.StartPages;

namespace Gro.ViewModels
{
    public class PageViewModel<T> : IPageViewModel<T> where T : PageData
    {
        public PageViewModel(T currentPage)
        {
            CurrentPage = currentPage;
        }

        public LayoutModel Layout { get; set; }

        public IContent Section { get; set; }

        public SettingsPage Settings { get; set; }

        public T CurrentPage { get; set; }

        public SiteUser User { get; set; }
    }

    public static class PageViewModel
    {
        /// <remarks>
        /// Convenience method for creating PageViewModels without having to specify the type as methods can use type inference while constructors cannot.
        /// </remarks>
        public static PageViewModel<T> Create<T>(T page) where T : PageData
        {
            return new PageViewModel<T>(page);
        }
    }
}
