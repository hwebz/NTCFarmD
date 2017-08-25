using EPiServer.SpecializedProperties;
using Gro.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;

namespace Gro.Helpers
{
    public static class LinkHelpers
    {
        /// <summary>
        ///     Writes an opening <![CDATA[ <a> ]]> tag to the response if the shouldWriteLink argument is true.
        ///     Returns a ConditionalLink object which when disposed will write a closing <![CDATA[ </a> ]]> tag
        ///     to the response if the shouldWriteLink argument is true.
        /// </summary>
        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink, IHtmlString url,
            string title = null, string cssClass = null)
        {
            if (!shouldWriteLink) return new ConditionalLink(helper.ViewContext, false);
            var linkTag = new TagBuilder("a");
            linkTag.Attributes.Add("href", url.ToHtmlString());

            if (!string.IsNullOrWhiteSpace(title))
            {
                linkTag.Attributes.Add("title", helper.Encode(title));
            }

            if (!string.IsNullOrWhiteSpace(cssClass))
            {
                linkTag.Attributes.Add("class", cssClass);
            }

            helper.ViewContext.Writer.Write(linkTag.ToString(TagRenderMode.StartTag));
            return new ConditionalLink(helper.ViewContext, true);
        }

        /// <summary>
        ///     Writes an opening <![CDATA[ <a> ]]> tag to the response if the shouldWriteLink argument is true.
        ///     Returns a ConditionalLink object which when disposed will write a closing <![CDATA[ </a> ]]> tag
        ///     to the response if the shouldWriteLink argument is true.
        /// </summary>
        /// <remarks>
        ///     Overload which only executes the delegate for retrieving the URL if the link should be written.
        ///     This may be used to prevent null reference exceptions by adding null checkes to the shouldWriteLink condition.
        /// </remarks>
        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink,
            Func<IHtmlString> urlGetter, string title = null, string cssClass = null)
        {
            IHtmlString url = MvcHtmlString.Empty;

            if (shouldWriteLink)
            {
                url = urlGetter();
            }

            return helper.BeginConditionalLink(shouldWriteLink, url, title, cssClass);
        }

        public class ConditionalLink : IDisposable
        {
            private readonly bool _linked;
            private readonly ViewContext _viewContext;
            private bool _disposed;

            public ConditionalLink(ViewContext viewContext, bool isLinked)
            {
                _viewContext = viewContext;
                _linked = isLinked;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;

                if (_linked)
                {
                    _viewContext.Writer.Write("</a>");
                }
            }
        }

        /// <summary>
        /// Find the corresponding links from a <see cref="LinkItemCollection"/>
        /// </summary>
        /// <param name="linkCollection">Collection of LinkItems</param>
        /// <returns>A collection of <see cref="GroLinkItem"/></returns>
        public static IEnumerable<GroLinkItem> GetGroLinkItems(this LinkItemCollection linkCollection)
            => linkCollection?.Select(GroLinkItem.FromLinkItem) ?? new GroLinkItem[] { };

        public static string GetFriendlyLinkOfPage(ContentReference contentRef)
        {
            if (ContentReference.IsNullOrEmpty(contentRef))
            {
                return string.Empty;
            }
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return url.ContentUrl(contentRef);
        }
    }
}
