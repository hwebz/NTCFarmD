using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class ArticlePageController : SiteControllerBase<ArticlePage>
    {
        public ViewResult Index(ArticlePage currentPage) => View("Index", new PageViewModel<ArticlePage>(currentPage));
    }
}
