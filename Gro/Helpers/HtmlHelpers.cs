using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Gro.Core.ContentTypes.Utils;
using System;
using System.IO;

namespace Gro.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Get GUID for Blockdata object
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model"></param>
        public static string Guid(this HtmlHelper htmlHelper, BlockData model) => (model as IContent)?.ContentGuid.ToString();

        /// <summary>
        ///     GetChildren and Filter result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="contentLink"></param>
        public static IEnumerable<T> GetChildrenFiltered<T>(this HtmlHelper htmlHelper, ContentReference contentLink)
            where T : IContent
        {
            if (ContentReference.IsNullOrEmpty(contentLink)) return Enumerable.Empty<T>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var children = contentLoader.GetChildren<T>(contentLink);
            return children.FilterForDisplay();
        }

        /// <summary>
        /// Get Typed Page from ContentReference
        /// </summary>
        /// <typeparam name="TContent"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="contentLink"></param>
        /// <returns></returns>
        public static TContent Get<TContent>(this HtmlHelper htmlHelper, ContentReference contentLink) where TContent : IContent
            => DataFactory.Instance.Get<TContent>(contentLink);

        public static bool HasChildren(this HtmlHelper htmlHelper, IContent page)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            if (page == null) return false;
            var children = contentLoader.GetChildren<PageData>(page.ContentLink).Any();
            return children;
        }

        /// <summary>
        /// Get the standard site-wide date time format string
        /// </summary>
        /// <param name="datetime"><see cref="DateTime"/> to display</param>
        /// <returns>Date time string</returns>
        public static string ToStandardDateTimeString(this DateTime datetime) => datetime.ToString("yyyy-MM-dd");

        /// <summary>
        /// Get the Swedish name of month
        /// </summary>
        /// <param name="datetime"><see cref="DateTime"/> to display</param>
        /// <returns>Month in Swedish</returns>
        public static string SwedishMonthString(this DateTime datetime) => datetime.ToString("MMM", new System.Globalization.CultureInfo("sv-SE"));

        public static string GetPushColorCss(this HtmlHelper htmlHelper, string color)
        {
            if (string.IsNullOrWhiteSpace(color)) return string.Empty;

            switch (color)
            {
                case PushColor.Green:
                    return "green-bg";
                case PushColor.Blue:
                    return "blue-bg";
                case PushColor.Orange:
                    return "orange-bg";
                default:
                    return "purple-bg";
            }
        }

        /// <summary>
        /// Renders the specified partial view to a string.
        /// </summary>
        /// <param name="controller">The current controller instance.</param>
        /// <param name="viewName">The name of the partial view.</param>
        /// <param name="model">The model.</param>
        /// <returns>The partial view as a string.</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                // Find the partial view by its name and the current controller context.
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

                // Create a view context.
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

                // Render the view using the StringWriter object.
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Check for debugging environment
        /// </summary>
        public static bool IsDebug(this HtmlHelper htmlHelper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
