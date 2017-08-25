using System.IO;
using System.Web;
using EPiServer.Core;

namespace Gro.Core.ContentTypes.Media
{
    public interface IMediaRepository
    {
        ContentReference CreateFolder(ContentReference parentReference, string folderName);
        ContentReference CreateFile(FileInfo fileInfo, ContentReference folderRef);
        ContentReference CreateFile(FileInfo fileInfo, string parentFolderName);
        ContentReference CreateFile(HttpPostedFileBase file, string parentFolderName, string fileName);
    }
}
