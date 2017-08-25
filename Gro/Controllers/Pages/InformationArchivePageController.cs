using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Business.Services.News;
using Gro.Core.ContentTypes.Pages;
using Gro.ViewModels.Pages.InformationArchive;

namespace Gro.Controllers.Pages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class InformationArchivePageController : SiteControllerBase<InformationArchivePage>
    {
        private readonly INewsService _newService;

        public InformationArchivePageController(INewsService newsService)
        {
            _newService = newsService;
        }

        public ViewResult Index(InformationArchivePage currentPage)
        {
            var model = new InformationArchivePageViewModel(currentPage)
            {
                LatestInformationPages = _newService.GetLatestInformationPages(currentPage.ContentLink, HttpContext)
            };

            return View("Index", model);
        }
    }
}
