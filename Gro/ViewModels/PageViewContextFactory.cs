using System.Linq;
using System.Web.Routing;
using EPiServer;
using EPiServer.Core;
using Gro.Core.ContentTypes.Pages.StartPages;

namespace Gro.ViewModels
{
    public class PageViewContextFactory
    {
        private readonly IContentLoader _contentLoader;
        public PageViewContextFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual LayoutModel CreateLayoutModel(ContentReference currentContentLink, RequestContext requestContext)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            return new LayoutModel
            {
                StartPageReference = startPage.ContentLink
            };
        }


        //Get the main section of the website, ie. subpage directly under start.
        public virtual IContent GetSection(ContentReference contentLink)
        {
            if (contentLink == null) return null;

            var currentContent = _contentLoader.Get<IContent>(contentLink);
            if (currentContent.ParentLink != null &&
                currentContent.ParentLink.CompareToIgnoreWorkID(ContentReference.StartPage))
            {
                return currentContent;
            }

            return _contentLoader.GetAncestors(contentLink)
                .OfType<PageData>()
                .SkipWhile(x => x.ParentLink == null || !x.ParentLink.CompareToIgnoreWorkID(ContentReference.StartPage))
                .FirstOrDefault();
        }
    }
}
