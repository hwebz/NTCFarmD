using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Business.Services.News;
using Gro.Core.ContentTypes.Pages;
using Gro.ViewModels.Pages.Information;

namespace Gro.Controllers.Pages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class InformationPageController : SiteControllerBase<InformationPage>
    {
        private readonly INewsService _newService;

        public InformationPageController(INewsService newsService)
        {
            _newService = newsService;
        }

        public ViewResult Index(InformationPage currentPage)
        {
            var archivePage = _newService.GetInformationArchivePage(currentPage);
            var model = new InformationPageViewModel(currentPage)
            {
                LatestInformationPages = _newService.GetLatestInformationPages(archivePage, HttpContext),
                ArchivePage = archivePage,
                HeadlineForListLatestPages = _newService.GetTitleForLatestInformatonPages(archivePage),
                SeemoreText = _newService.GetSeemoreTextForLatestInformationPages(archivePage),
            };
            return View("Index", model);
        }
    }
}
