using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Gro.Business.Services.News;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Business.Services.Users;

namespace Gro.ViewModels
{
    public class PageContextActionFilter : IResultFilter
    {
        private readonly PageViewContextFactory _contextFactory;
        private readonly IUserManagementService _userManager;
        private readonly IContentLoader _contentLoader;
        private readonly IGroContentDataService _contentDataService;

        public PageContextActionFilter(IUserManagementService userManager, IContentLoader contentLoader, IGroContentDataService contentDataService)
        {
            _userManager = userManager;
            _contentLoader = contentLoader;
            _contentDataService = contentDataService;
            _contextFactory = new PageViewContextFactory(contentLoader);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;

            var viewModel = model as IPageViewModel<PageData>;
            if (viewModel == null) return;

            var contentReference = filterContext.RequestContext.GetContentLink();

            PrepareLayout(filterContext, contentReference, viewModel);

            PopulateSiteUser(filterContext, viewModel);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private void PopulateSiteUser(ControllerContext filterContext, IPageViewModel<PageData> viewModel)
        {
            var siteUser = _userManager.GetSiteUser(filterContext.HttpContext);
            viewModel.User = siteUser;
        }

        private void PrepareLayout(ControllerContext filterContext, ContentReference contentReference, IPageViewModel<PageData> viewModel)
        {
            viewModel.Layout = viewModel.Layout ?? _contextFactory.CreateLayoutModel(contentReference, filterContext.RequestContext);

            if (viewModel.Section == null)
            {
                viewModel.Section = _contextFactory.GetSection(contentReference);
            }

            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            if (!PageReference.IsNullOrEmpty(startPage.SettingsPage))
            {
                viewModel.Settings = _contentLoader.Get<SettingsPage>(startPage.SettingsPage);
            }
            else
            {
                throw new EPiServerException("You need to set the property for settingspage on startpage");
            }
        }
    }
}
