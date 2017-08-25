using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.ContentTypes.Pages.StartPages;

namespace Gro.Core.ContentTypes.Utils
{
    public static class ContentExtensions
    {
        private static SettingsPage _settingPage;

        public static SettingsPage GetSettingsPage()
        {
            if (PageReference.IsNullOrEmpty(ContentReference.StartPage)) return null;

            var startPage = ContentReference.StartPage.Get<StartPage>();

            if (PageReference.IsNullOrEmpty(startPage.SettingsPage)) return null;
            var settingsPage = startPage.SettingsPage.Get<SettingsPage>();
            _settingPage = settingsPage;

            return _settingPage;
        }

        public static string GetExternalUrl(this IContent content)
        {
            if (content == null) return string.Empty;

            var internalUrl = UrlResolver.Current.GetUrl(content.ContentLink);
            return string.IsNullOrWhiteSpace(internalUrl) ? "#" : internalUrl;
        }

        public static string GetExternalUrl(this ContentReference contentRef)
        {
            if (contentRef == null || ContentReference.IsNullOrEmpty(contentRef))
            {
                return string.Empty;
            }
            var internalUrl = UrlResolver.Current.GetUrl(contentRef);
            return string.IsNullOrWhiteSpace(internalUrl) ? "#" : internalUrl;
        }

        /// <summary>
        /// Shorthand for DataFactory.Instance.Get
        /// </summary>
        public static TContent Get<TContent>(this ContentReference contentLink) where TContent : IContent
            => DataFactory.Instance.Get<TContent>(contentLink);

        /// <summary>
        /// Filters content which should not be visible to the user.
        /// </summary>
        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> contents, bool requirePageTemplate = false,
            bool requireVisibleInMenu = false)
            where T : IContent
        {
            var accessFilter = new FilterAccess();
            var publishedFilter = new FilterPublished();
            contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));
            if (requirePageTemplate)
            {
                var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Page;
                contents = contents.Where(x => !templateFilter.ShouldFilter(x));
            }
            if (requireVisibleInMenu)
            {
                contents = contents.Where(x => VisibleInMenu(x));
            }
            return contents;
        }

        /// <summary>
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static bool VisibleInMenu(IContent content) => (content as PageData)?.VisibleInMenu ?? true;

        public static IList<T> Withdraw<T>(this IList<T> list, Func<T, bool> selector)
        {
            IList<T> result = list.Where(selector).ToList();
            foreach (var item in result)
            {
                list.Remove(item);
            }
            return result;
        }

        /// <summary>
        /// GetLevel
        /// To get level of the page, all Icontainer Page will be excluded out of this calculation.
        ///  - Root: level 0;  Start page: level 1
        /// </summary>
        /// <param name="currentpage">Page to get the level</param>
        /// <returns>The Level of the page</returns>
        public static int GetLevel(this PageData currentpage)
        {
            var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            return contentRepo.GetAncestors(currentpage.ContentLink)
                //.Where(x => !(x is Pages.ContainerPages.IContainerPage))
                .Select(x => x).Count();
        }

        public static string GetCleanExternalUrl(this Url url)
        {
            if (url == null || url.IsEmpty()) return string.Empty;

            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();

            // Convert it to an EPiServer internal URL to be passed into a new EPiServer UrlBuilder object
            var internalUrl = urlResolver.GetPermanent(url.ToString(), false);

            //If GetPermanent returns empty string, this means that this is an external link
            //So we return the link directly 
            if (string.IsNullOrEmpty(internalUrl)) return url.ToString();

            var urlBuilder = new UrlBuilder(internalUrl);

            // Get the clean URL with a context mode of default (i.e. view mode)
            var externalUrl = urlResolver.GetUrl(urlBuilder, ContextMode.Default);
            return externalUrl;
        }

        public static UserAgreementsPage GetUserAgreementPage()
        {
            var userAgreementRef = GetSettingsPage()?.UserAgreementPage;
            var userAgreementPage = userAgreementRef?.Get<PageData>();
            return userAgreementPage as UserAgreementsPage;
        }

        public static string GetPageUnderSettingUrl(Func<SettingsPage, ContentReference> pageGetter)
        {
            var settingPage = GetSettingsPage();
            if (settingPage == null || pageGetter == null) return null;

            var page = pageGetter(settingPage);
            return ServiceLocator.Current.GetInstance<UrlResolver>().GetVirtualPath(page)?.GetUrl();
        }

        public static string GetStartPageUrl() => ServiceLocator.Current.GetInstance<UrlResolver>().GetVirtualPath(ContentReference.StartPage).GetUrl();

        public static string GetCustomerCardUrl()
        {
            var settingPage = GetSettingsPage();
            var customerCardRef = settingPage != null ? settingPage.CustomerCardPage : ContentReference.EmptyReference;
           return customerCardRef!=ContentReference.EmptyReference? ServiceLocator.Current.GetInstance<UrlResolver>().GetVirtualPath(customerCardRef).GetUrl():string.Empty;
        }

        public static IContent GetContent(this LinkItem linkItem)
            => linkItem.UrlResolver.Service.Route(new EPiServer.UrlBuilder(linkItem.GetMappedHref()));
    }
}
