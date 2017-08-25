using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Gro.Core.ContentTypes.Media;
using System;

namespace Gro.Business
{
    public class GroLinkItem
    {
        private readonly LinkItem _linkItem;

        private GroLinkItem(LinkItem linkItem, GroLinkType linkType)
        {
            _linkItem = linkItem;
            Type = linkType;
        }

        /// <summary>
        /// Create a <see cref="GroLinkItem"/> from a <see cref="LinkItem"/> in the system
        /// </summary>
        /// <param name="linkItem"><see cref="LinkItem"/> instance</param>
        /// <returns><see cref="GroLinkItem"/> instance</returns>
        public static GroLinkItem FromLinkItem(LinkItem linkItem) => new GroLinkItem(linkItem, GetLinkType(linkItem));

        /// <summary>
        /// Gro link item type
        /// </summary>
        public GroLinkType Type { get; private set; }

        /// <summary>
        /// Get the <see cref="LinkItem"/> instance 
        /// </summary>
        public LinkItem AsLinkItem() => _linkItem;

        private static IContent GetContent(LinkItem linkItem)
            => linkItem.UrlResolver.Service.Route(new EPiServer.UrlBuilder(linkItem.GetMappedHref()));

        private static GroLinkType GetLinkType(LinkItem linkItem)
        {
            if (linkItem?.Href?.StartsWith("mailto:") == true)
            {
                return GroLinkType.Email;
            }

            var content = GetContent(linkItem);
            if (content == null)
            {
                //cannot find a content in the system, return link type as external link
                return GroLinkType.ExternalLink;
            }
            if (content is ImageData)
            {
                return GroLinkType.Image;
            }
            if (content is PageData)
            {
                return GroLinkType.InternalLink;
            }
            if (!(content is GroFile))
            {
                throw new NotSupportedException($"Content type is not supported: {content.GetType().Name}");
            }

            var mimeType = (content as GroFile).MimeType;
            if (mimeType == "application/pdf")
            {
                return GroLinkType.Pdf;
            }
            if (mimeType.Contains("application/vnd.ms") || mimeType.Contains("application/vnd.openxml") ||
                mimeType.Contains("application/ms"))
            {
                return GroLinkType.Document;
            }

            throw new NotSupportedException($"Content type is not supported: {content.GetType().Name}");
        }

        public string GetHref()
        {
            if (Type == GroLinkType.ExternalLink || Type == GroLinkType.Email)
            {
                return _linkItem.GetMappedHref();
            }
            var content = GetContent(_linkItem);
            var link = _linkItem.UrlResolver.Service.GetUrl(content);

            return link;
        }
    }
}
