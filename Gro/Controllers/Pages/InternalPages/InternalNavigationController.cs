using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.News;
using Gro.Core.ContentTypes.Pages.InternalPages;
using Gro.Core.ContentTypes.Utils;
using Gro.ViewModels.Navigation;

namespace Gro.Controllers.Pages.InternalPages
{
    public class InternalNavigationController : Controller
    {
        private readonly IGroContentDataService _groContentDataService;
        private readonly IContentRepository _contentRepository;

        public InternalNavigationController(IGroContentDataService groContentDataService, IContentRepository contentRepository)
        {
            _groContentDataService = groContentDataService;
            _contentRepository = contentRepository;
        }

        public ActionResult Index(PageData currentPage)
        {
            var model = new InternalNavigationItemModel();
            var settingPage = ContentExtensions.GetSettingsPage();
            var startpageReference = settingPage?.InternalStartPage;
            if (startpageReference == null)
            {
                throw new NullReferenceException("Reference to internal start page in settings is not set");
            }

            var startPage = _contentRepository.Get<InternalStartPage>(startpageReference);
            model.Page = startPage;
            model.IsActive = currentPage?.ContentLink.ID == startPage?.ContentLink.ID;
            var childPages = _groContentDataService.GetChildrenForVisitor<PageData>(startPage, HttpContext);
            model.SubItems.AddRange(childPages.Select(x => GetSubItems(x, currentPage, x.URLSegment)));
            return PartialView("~/Views/InternalPages/_InternalNavigation.cshtml", model);
        }

        private InternalNavigationItemModel GetSubItems(PageData page, PageData currentPage, string cssClass)
        {
            var model = new InternalNavigationItemModel();
            model.IsActive = model.IsActive = currentPage?.ContentLink.ID == page?.ContentLink.ID;
            model.Page = page;
            model.CssClass = cssClass;
            var subItems = _groContentDataService.GetChildrenForVisitor<PageData>(page, HttpContext);
            model.SubItems.AddRange(subItems.Select(x => GetSubItems(x, currentPage, "")));

            return model;
        }
    }
}