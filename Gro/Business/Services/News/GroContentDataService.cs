using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using Gro.Business.Services.Users;
using System.Web;
using EPiServer.SpecializedProperties;
using Gro.Business.Rendering;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.Interfaces;

namespace Gro.Business.Services.News
{
    public class GroContentDataService : IGroContentDataService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUserManagementService _usersManagementService;
        private readonly IOrganizationUserRepository _orgUserRepo;

        public GroContentDataService(
            IContentRepository contentRepository,
            IUserManagementService usersManagementService,
            IOrganizationUserRepository orgUserRepo)
        {
            _contentRepository = contentRepository;
            _usersManagementService = usersManagementService;
            _orgUserRepo = orgUserRepo;
        }

        public IEnumerable<TContent> GetSiblingsForVisitor<TContent>(PageData page, HttpContextBase httpContext, bool excludeInvisible) where TContent : PageData
        {
            var siblings = _contentRepository
                .GetChildren<TContent>(page.ParentLink)
                .Where(p => PageAccess.CanAccessPage(_usersManagementService, _orgUserRepo, p, httpContext))
                .Where(p => p.VisibleInMenu && p.CheckPublishedStatus(PagePublishedStatus.Published));

            return siblings;
        }

        public IEnumerable<TContent> GetChildrenForVisitor<TContent>(PageData page, HttpContextBase httpContext, bool excludePageInvisibleInNav = true)
            where TContent : PageData
        {
            var children = _contentRepository
                .GetChildren<TContent>(page.ContentLink)
                .Where(p => PageAccess.CanAccessPage(_usersManagementService, _orgUserRepo, p, httpContext))
                .Where(p => p.VisibleInMenu && p.CheckPublishedStatus(PagePublishedStatus.Published))
                .ToArray();

            return children;
        }

        public IEnumerable<GroLinkItem> PopulateLinkItems(LinkItemCollection linkCollection, HttpContextBase httpContext)
        {
            var result = new List<GroLinkItem>();
            if (linkCollection == null) return result;
            foreach (var item in linkCollection)
            {
                var linkItem = GroLinkItem.FromLinkItem(item);
                if (linkItem.Type != GroLinkType.InternalLink)
                {
                    result.Add(linkItem);
                    continue;
                }
                var content = item.GetContent();
                if (content is PageData && !PageAccess.CanAccessPage(_usersManagementService, _orgUserRepo, (PageData)content, httpContext))
                {
                    continue;
                }
                result.Add(linkItem);
            }
            return result;
        }

        public TContent FindAncestor<TContent>(IContent content) where TContent : PageData
        {
            if (content == null) return null;

            return _contentRepository.GetAncestors(content.ContentLink).FirstOrDefault(p => p is TContent) as TContent;
        }
    }
}
