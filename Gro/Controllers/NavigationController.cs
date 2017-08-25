using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.News;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.BasePages;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.ContentTypes.Utils;
using Gro.ViewModels.Navigation;
using System;

namespace Gro.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IGroContentDataService _groContentDataService;
        private readonly IContentRepository _contentRepository;

        public NavigationController(IGroContentDataService groContentDataService, IContentRepository contentRepository)
        {
            _groContentDataService = groContentDataService;
            _contentRepository = contentRepository;
        }

        public ActionResult GetTopNav(SitePageBase page)
        {
            //var listServiceStartPages = _groContentDataService.GetChildrenByVisitor<ServiceStartPage>(ContentReference.StartPage, true);
            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);

            var listServiceStartPages = _groContentDataService.GetChildrenForVisitor<ServiceStartPage>(startPage,
                HttpContext);
            var model = new TopNavigationModel();

            var serviceStartPages = listServiceStartPages as ServiceStartPage[] ?? listServiceStartPages.ToArray();
            if (serviceStartPages.Any())
            {
                model.ListServiceStartPageItems.AddRange(serviceStartPages.Select(item => new NavigationItemModel
                {
                    ContentLink = item.ContentLink,
                    Text = item.Name,
                    CssClass = GetNavCssClassForService(item)
                }));
            }

            // get service name
            var topLevelServiceStartPage = page as ServiceStartPage ?? _groContentDataService.FindAncestor<ServiceStartPage>(page);
            var startPageRef = topLevelServiceStartPage?.ContentLink ?? ContentReference.EmptyReference;
            var startpageText = topLevelServiceStartPage?.Name ?? string.Empty;

            if (string.IsNullOrEmpty(startpageText))
            {
                var topLevelStartPage = page as StartPage ?? _groContentDataService.FindAncestor<StartPage>(page);
                if (topLevelStartPage != null && !ContentReference.IsNullOrEmpty(topLevelStartPage.SettingsPage))
                {
                    var settingPage = _contentRepository.Get<PageData>(topLevelStartPage.SettingsPage);
                    startpageText = (settingPage as SettingsPage)?.SelectServiceText ?? string.Empty;
                    startPageRef = topLevelStartPage.ContentLink;
                }
            }
            model.StartPageText = startpageText;
            model.StartpageReference = startPageRef;

            return PartialView("Navigations/_TopNavigation", model);
        }

        //private string GetNavCssClassForService(ServiceStartPage serviceStartPage) => serviceStartPage.GetServiceTypeName().ToLower();
        private static string GetNavCssClassForService(ServiceStartPage serviceStartPage)
        {
            if (serviceStartPage is OdlingStartPage)
            {
                return "odling";
            }
            if (serviceStartPage is SpannmalStartPage)
            {
                return "spannmal";
            }
            if (serviceStartPage is EkonomiStartPage)
            {
                return "ekonomi";
            }
            if (serviceStartPage is EkonomiIframeStartPage)
            {
                return "ekonomi";
            }
            if (serviceStartPage is FoderStartPage)
            {
                return "foder";
            }
            if (serviceStartPage is MaskinStartPage)
            {
                return "maskiner";
            }
            return string.Empty;
        }

        public ActionResult GetSubNav(SitePageBase page)
        {
            var startPage = (page as ServiceStartPage) ?? _groContentDataService.FindAncestor<ServiceStartPage>(page);
            var directSubNavigation = (PageData)_groContentDataService.FindAncestor<FolderPage>(page) ?? page;
            var navigationItems = startPage == null
                ? new NavigationItemModel[0]
                : _groContentDataService
                    .GetChildrenForVisitor<PageData>(startPage, HttpContext)
                    .Select(startPageChild => new NavigationItemModel
                    {
                        ContentLink = startPageChild.ContentLink,
                        Text = startPageChild.Name,
                        IsActive = directSubNavigation.ContentGuid == startPageChild.ContentGuid,
                        Children = _groContentDataService
                            .GetChildrenForVisitor<SitePageBase>(startPageChild, HttpContext)
                            .Select(c => new NavigationItemModel
                            {
                                Text = c.Name,
                                ContentLink = c.ContentLink,
                                //IsActive = c.ContentGuid == page.ContentGuid,
                                IsActive = IsSelfOrDirectChild(c, page),
                                Children = _groContentDataService.GetChildrenForVisitor<SitePageBase>(c, HttpContext)
                                    .Select(subChild => new NavigationItemModel
                                    {
                                        Text = subChild.Name,
                                        ContentLink = subChild.ContentLink,
                                        IsActive = IsSelfOrDirectChild(subChild, page),
                                    })
                                    .ToArray()
                            })
                            .ToArray()
                    })
                    .ToArray();

            return PartialView("Navigations/_SubNavigation", navigationItems);
        }

        #region Right navigation model

        public ActionResult GetRightNav(SitePageBase page, int minLevel = 3, Type[] pageTypes = null)
        {
            var rightNavModel = GetRightNavModel(page, minLevel, pageTypes);
            return PartialView("_RightNavigation", rightNavModel);
        }

        private RightNavigationModel GetRightNavModel(PageData currentPage, int minLevel, Type[] pageTypes)
        {
            var model = new RightNavigationModel(currentPage is NonServicePageBase)
            {
                ListNavigationItems = GetRightNavListItems(currentPage, minLevel, pageTypes).ToList()
            };
            var nonServicePage = currentPage as NonServicePageBase;
            if (nonServicePage != null)
            {
                model.Header = GetRightNavHeader(nonServicePage);
            }
            return model;
        }

        private static string GetRightNavHeader(NonServicePageBase currentPage)
        {
            if (!string.IsNullOrEmpty(currentPage.RightNavigationHeader))
            {
                return currentPage.RightNavigationHeader;
            }

            var parent = currentPage.ParentLink.Get<PageData>();
            var parentWithRightNavHeader = parent as NonServicePageFolder;
            return parentWithRightNavHeader != null ? parentWithRightNavHeader.RightNavigationHeader : string.Empty;
        }

        private IEnumerable<NavigationItemModel> GetRightNavListItems(PageData currentPage, int minLevel, Type[] pageTypes)
        {
            var pageLevel = currentPage.GetLevel();
            var result = new List<NavigationItemModel>();
            if (pageLevel < minLevel) return result;

            var childrenOfCurrPage = _groContentDataService.GetChildrenForVisitor<SitePageBase>(currentPage, HttpContext);

            IEnumerable<SitePageBase> listSiblingsAtRootLevel;

            if (childrenOfCurrPage.Any() || pageLevel <= 4)
            {
                //This page will be the one of main nav item together with its siblings.
                listSiblingsAtRootLevel = _groContentDataService.GetSiblingsForVisitor<SitePageBase>(currentPage, HttpContext);
            }
            else
            {
                // this page is the leaf, go up ,find its parent
                var parentPage = _contentRepository.Get<PageData>(currentPage.ParentLink);
                listSiblingsAtRootLevel = _groContentDataService.GetSiblingsForVisitor<SitePageBase>(parentPage ?? currentPage, HttpContext);
            }

            if (pageTypes != null)
            {
                listSiblingsAtRootLevel = listSiblingsAtRootLevel
                    .Where(page => pageTypes.Any(type => type.IsInstanceOfType(page)));
            }

            result.AddRange(listSiblingsAtRootLevel.Select(page => new NavigationItemModel
            {
                ContentLink = page.ContentLink,
                IsActive = page.ContentGuid == currentPage.ContentGuid,
                Text = page.Name,
                Children = _groContentDataService
                    .GetChildrenForVisitor<SitePageBase>(page, HttpContext)
                    .Select(x => CreateNavItem(x, currentPage))
            }));

            return result;
        }

        private static NavigationItemModel CreateNavItem(IContent page, PageData topLevelPage) => new NavigationItemModel
        {
            ContentLink = page.ContentLink,
            IsActive = IsSelfOrDirectChild(page, topLevelPage),
            Text = page.Name
        };

        private static bool IsSelfOrDirectChild(IContent page, PageData targetPage)
            => page.ContentGuid.Equals(targetPage.ContentGuid) || page.ContentLink.Equals(targetPage.ParentLink);

        #endregion
    }
}
