using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using Gro.Business.Rendering;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.Interfaces;

namespace Gro.Business.Services.News
{
    public class NewsService : INewsService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IOrganizationUserRepository _organizationRepo;

        public NewsService(IContentRepository contentRepository, IUserManagementService userManagementService, IOrganizationUserRepository organizationRepo)
        {
            _contentRepository = contentRepository;
            _userManagementService = userManagementService;
            _organizationRepo = organizationRepo;
        }

        public IEnumerable<InformationPage> GetLatestInformationPages(InformationPage inforPage , HttpContextBase httpContext, int count = 0,  bool filterByAccessRights = true)
        {
            return inforPage == null ? new List<InformationPage>() : GetLatestInformationPages(inforPage.ParentLink,httpContext, count, filterByAccessRights);
        }

        public IEnumerable<InformationPage> GetLatestInformationPages(ContentReference archivePage, HttpContextBase httpContext, int count = 0, bool filterByAccessRights = true)
        {
            if (archivePage == ContentReference.EmptyReference || archivePage == null)
            {
                return new List<InformationPage>();
            }

            var result = _contentRepository
                .GetChildren<InformationPage>(archivePage)
                .Where(page => PageAccess.CanAccessPage(_userManagementService, _organizationRepo, page, httpContext))
                .OrderByDescending(x => x.StartPublish);

            return count > 0
                ? FilterForVisitor.Filter(result).Take(count).Cast<InformationPage>().ToList()
                : FilterForVisitor.Filter(result).Cast<InformationPage>().ToList();
            //return count > 0 ? result.Take(count).ToList() : result.ToList();
        }

        public ContentReference GetInformationArchivePage(InformationPage inforPage)
            => inforPage?.ParentLink ?? ContentReference.EmptyReference;

        public string GetTitleForLatestInformatonPages(ContentReference archivePage)
        {
            var archivePageData = _contentRepository.Get<InformationArchivePage>(archivePage);
            return archivePageData != null ? archivePageData.LatestPagesHeadline : string.Empty;
        }

        public string GetSeemoreTextForLatestInformationPages(ContentReference archivePage)
        {
            var archivePageData = _contentRepository.Get<InformationArchivePage>(archivePage);
            return archivePageData != null ? archivePageData.LatestPagesSeemoreText : string.Empty;
        }
    }
}
