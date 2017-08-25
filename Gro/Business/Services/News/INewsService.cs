using System.Collections.Generic;
using System.Web;
using EPiServer.Core;
using Gro.Core.ContentTypes.Pages;

namespace Gro.Business.Services.News
{
    public interface INewsService
    {
        IEnumerable<InformationPage> GetLatestInformationPages(InformationPage inforPage, HttpContextBase httpContext, int count = 0, bool filterByAccessRights = true);
        IEnumerable<InformationPage> GetLatestInformationPages(ContentReference archivePage, HttpContextBase httpContext, int count = 0, bool filterByAccessRights = true);
        ContentReference GetInformationArchivePage(InformationPage inforPage);
        string GetTitleForLatestInformatonPages(ContentReference archivePage);
        string GetSeemoreTextForLatestInformationPages(ContentReference archivePage);
    }
}
