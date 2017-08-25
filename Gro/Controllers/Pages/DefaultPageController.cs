using System;
using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer;
using EPiServer.Framework.DataAnnotations;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.ViewModels;

namespace Gro.Controllers.Pages
{
    /// <summary>
    /// Concrete controller that handles all page types that don't have their own specific controllers.
    /// </summary>
    /// <remarks>
    /// Note that as the view file name is hard coded it won't work with DisplayModes (ie Index.mobile.cshtml).
    /// For page types requiring such views add specific controllers for them. Alterntively the Index action
    /// could be modified to set ControllerContext.RouteData.Values["controller"] to type name of the currentPage
    /// argument. That may however have side effects.
    /// </remarks>
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class DefaultPageController : SiteControllerBase<SitePageBase>
    {
        public virtual ViewResult Index(SitePageBase currentPage)
        {
            //var x = await wRepo.GetAnalyzeListAsync("1", 2, "3");
            var model = CreateModel(currentPage);

            //Sets metadata information
            ViewBag.Title = currentPage.Seo.MetaTitle ?? currentPage.Name;
            //ViewBag.Description = currentPage.Seo.MetaDescription ?? currentPage.MainIntro;

            ViewBag.Keywords = currentPage.Seo.MetaKeywords;
            return View($"~/Views/{currentPage.GetOriginalType().Name}/Index.cshtml", model);
        }

        /// <summary>
        /// Creates a PageViewModel where the type parameter is the type of the page.
        /// </summary>
        /// <remarks>
        /// Used to create models of a specific type without the calling method having to know that type.
        /// </remarks>
        private static IPageViewModel<SitePageBase> CreateModel(SitePageBase page)
        {
            var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
            return Activator.CreateInstance(type, page) as IPageViewModel<SitePageBase>;
        }
    }
}
