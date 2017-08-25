using System;
using System.IO;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace Gro.Core.ContentTypes.Media
{
    /// <summary>
    /// Implement store file using File Blob provider
    /// </summary>
    public class FileBlobRepository : IMediaRepository
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private readonly IContentRepository _contentRepository;
        private readonly IContentTypeRepository _contentTypeRepository;

        public FileBlobRepository(IContentRepository contentRepository, IContentTypeRepository contentTypeRepository)
        {
            _contentRepository = contentRepository;
            _contentTypeRepository = contentTypeRepository;
        }

        /// <summary>
        /// Creat media data in folder
        /// </summary>
        /// <param name="fileInfo">File input</param>
        /// <param name="folderRef">Content reference to parent folder</param>
        public ContentReference CreateFile(FileInfo fileInfo, ContentReference folderRef)
        {
            //try
            //{
            using (var fileStream = fileInfo.OpenRead())
            {
                var mediaDataResolver = ServiceLocator.Current.GetInstance<ContentMediaResolver>();
                var blobFactory = ServiceLocator.Current.GetInstance<BlobFactory>();

                //Get extension filename
                var fileExtension = Path.GetExtension(fileInfo.Name); // ex. .jpg or .txt

                var media = (from d in _contentRepository.GetChildren<MediaData>(folderRef)
                             where string.Compare(d.Name, fileInfo.Name, StringComparison.OrdinalIgnoreCase) == 0
                             select d).FirstOrDefault();

                if (media == null)
                {
                    //Get a suitable MediaData type from extension
                    var mediaType = mediaDataResolver.GetFirstMatching(fileExtension);
                    var contentType = _contentTypeRepository.Load(mediaType);
                    //Get a new empty file data
                    media = _contentRepository.GetDefault<MediaData>(folderRef, contentType.ID);
                    media.Name = fileInfo.Name;
                }
                else
                {
                    media = media.CreateWritableClone() as MediaData;
                }

                //Create a blob in the binary container
                if (media != null)
                {
                    var blob = blobFactory.CreateBlob(media.BinaryDataContainer, fileExtension);
                    blob.Write(fileStream);

                    //Assign to file and publish changes
                    media.BinaryData = blob;
                }
                var fileRef = _contentRepository.Save(media, SaveAction.Publish, AccessLevel.NoAccess);

                return fileRef;
            }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error("Method CreateFile(FileInfo fileInfo, ContentReference folderRef) Unhandle Exception: {0}", ex.Message);
            //    return null;
            //}
        }

        public ContentReference CreateFile(FileInfo fileInfo, string parentFolderName)
        {
            try
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    var mediaDataResolver = ServiceLocator.Current.GetInstance<ContentMediaResolver>();
                    var blobFactory = ServiceLocator.Current.GetInstance<BlobFactory>();

                    //Get extension filename
                    var fileExtension = Path.GetExtension(fileInfo.Name); // ex. .jpg or .txt
                    var folderRef = CreateFolder(SiteDefinition.Current.GlobalAssetsRoot, parentFolderName);

                    var media = (from d in _contentRepository.GetChildren<MediaData>(folderRef)
                                 where string.Compare(d.Name, fileInfo.Name, StringComparison.OrdinalIgnoreCase) == 0
                                 select d).FirstOrDefault();

                    if (media == null)
                    {
                        //Get a suitable MediaData type from extension
                        var mediaType = mediaDataResolver.GetFirstMatching(fileExtension);
                        var contentType = _contentTypeRepository.Load(mediaType);
                        //Get a new empty file data
                        media = _contentRepository.GetDefault<MediaData>(folderRef, contentType.ID);
                        media.Name = fileInfo.Name;
                    }
                    else
                    {
                        media = media.CreateWritableClone() as MediaData;
                    }

                    //Create a blob in the binary container
                    if (media != null)
                    {
                        var blob = blobFactory.CreateBlob(media.BinaryDataContainer, fileExtension);
                        blob.Write(fileStream);

                        //Assign to file and publish changes
                        media.BinaryData = blob;
                    }
                    var fileRef = _contentRepository.Save(media, SaveAction.Publish, AccessLevel.NoAccess);

                    return fileRef;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Method CreateFile(FileInfo fileInfo, string parentFolderName) Unhandle Exception: {0}", ex.Message);
                return null;
            }
        }

        public ContentReference CreateFile(HttpPostedFileBase file, string parentFolderName, string fileName)
        {
            try
            {
                var mediaDataResolver = ServiceLocator.Current.GetInstance<ContentMediaResolver>();
                var blobFactory = ServiceLocator.Current.GetInstance<BlobFactory>();

                //Get extension filename
                var fileExtension = Path.GetExtension(file.FileName); // ex. .jpg or .txt
                var folderRef = CreateFolder(SiteDefinition.Current.GlobalAssetsRoot, parentFolderName);

                //
                var media = (from d in _contentRepository.GetChildren<MediaData>(folderRef)
                             where string.Compare(d.Name, fileName, StringComparison.OrdinalIgnoreCase) == 0
                             select d).FirstOrDefault();

                if (media == null)
                {
                    //Get a suitable MediaData type from extension
                    var mediaType = mediaDataResolver.GetFirstMatching(fileExtension);
                    var contentType = _contentTypeRepository.Load(mediaType);
                    //Get a new empty file data
                    media = _contentRepository.GetDefault<MediaData>(folderRef, contentType.ID);
                    media.Name = fileName;
                }
                else
                {
                    media = media.CreateWritableClone() as MediaData;
                }

                //Create a blob in the binary container
                if (media != null)
                {
                    var blob = blobFactory.CreateBlob(media.BinaryDataContainer, fileExtension);
                    blob.Write(file.InputStream);

                    //Assign to file and publish changes
                    media.BinaryData = blob;
                }
                var fileRef = _contentRepository.Save(media, SaveAction.Publish, AccessLevel.NoAccess);

                return fileRef;
            }
            catch (Exception ex)
            {
                Logger.Error("Method CreateFile(HttpPostedFileBase file, string parentFolderName, string fileName) Unhandle Exception: {0}", ex.Message);
                return null;
            }
        }

        public ContentReference CreateFolder(ContentReference parentReference, string folderName)
        {
            try
            {
                if (parentReference == null) parentReference = SiteDefinition.Current.GlobalAssetsRoot;

                var subFolder = (from d in _contentRepository.GetChildren<ContentFolder>(parentReference)
                                 where string.Compare(d.Name, folderName, StringComparison.OrdinalIgnoreCase) == 0
                                 select d).FirstOrDefault();

                if (subFolder == null)
                {
                    subFolder = _contentRepository.GetDefault<ContentFolder>(parentReference);
                    subFolder.Name = folderName;

                    var folderRef = _contentRepository.Save(subFolder, SaveAction.Publish, AccessLevel.NoAccess);

                    subFolder = _contentRepository.Get<ContentFolder>(folderRef);
                }

                return subFolder.ContentLink;
            }
            catch (Exception ex)
            {
                Logger.Error("Method CreateSubFolder Unhandle Exception: {0}", ex.Message);
                return null;
            }
        }
    }
}
