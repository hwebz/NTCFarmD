using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Gro.Core.ContentTypes.Pages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class ExternalArticlePageController : PageController<ExternalArticlePage>
    {
        public ViewResult Index(ExternalArticlePage currentPage) => View("Index", new PageViewModel<ExternalArticlePage>(currentPage));
    }
}
