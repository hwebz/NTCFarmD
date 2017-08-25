using Gro.Core.Interfaces;
using System;
using System.Threading.Tasks;
using System.IO;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Web;
using EPiServer.DataAccess;
using System.Linq;
using EPiServer.Security;
using System.Collections.Generic;

namespace Gro.Core.ContentTypes.Business
{
    public class EpiserverFileRepository : IFileRepository
    {
        private readonly IContentRepository _contentRepo;
        private readonly IContentTypeRepository _contentTypeRepo;
        private readonly ContentMediaResolver _mediaResolver;
        private readonly BlobFactory _blobFactory;

        public EpiserverFileRepository(
            IContentRepository contentRepo,
            IContentTypeRepository contentTypeRepo,
            ContentMediaResolver mediaResolver,
            BlobFactory blobFactory)
        {
            _contentRepo = contentRepo;
            _mediaResolver = mediaResolver;
            _contentTypeRepo = contentTypeRepo;
            _blobFactory = blobFactory;
        }

        public Task<string> SaveAsync(Stream fileStream, string extension, string fileGroups, string owner)
        {
            if (string.IsNullOrWhiteSpace(fileGroups)) throw new ArgumentNullException(nameof(fileGroups));
            if (string.IsNullOrWhiteSpace(extension)) throw new ArgumentNullException(nameof(extension));
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));

            var fileGroupArray = fileGroups.Split('/').Where(n => !string.IsNullOrWhiteSpace(n));
            var groupArray = fileGroupArray as string[] ?? fileGroupArray.ToArray();
            var groupFolder = GetGroupFolder(groupArray);

            //Get a suitable MediaData type from extension
            var mediaType = _mediaResolver.GetFirstMatching(extension);
            var contentType = _contentTypeRepo.Load(mediaType);
            var mediaData = _contentRepo.GetDefault<MediaData>(groupFolder.ContentLink, contentType.ID);

            var fileName = $"{DateTime.Now.Ticks}_{Guid.NewGuid().ToString()}{extension}";
            mediaData.Name = fileName;
            mediaData.CreatedBy = owner;

            //Create a blob in the binary container
            var blob = _blobFactory.CreateBlob(mediaData.BinaryDataContainer, extension);
            blob.Write(fileStream);

            //Assign to file and publish changes
            mediaData.BinaryData = blob;
            _contentRepo.Save(mediaData, SaveAction.Publish, AccessLevel.NoAccess);
            return Task.FromResult($"/globalassets/{string.Join("/", groupArray)}/{fileName}");
        }

        public Task DeleteAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return Task.FromResult(0);

            var urlSplit = url.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            if (urlSplit.Length < 2) throw new InvalidOperationException("Wrong url format");

            var groupFolder = GetGroupFolder(urlSplit.Skip(1).Take(urlSplit.Length - 2));
            if (groupFolder == null) return Task.FromResult(0);

            var file = _contentRepo
                .GetChildren<MediaData>(groupFolder.ContentLink)
                .FirstOrDefault(m => m.Name == urlSplit.Last());
            if (file == null) return Task.FromResult(0);

            _blobFactory.Delete(file.BinaryData.ID);
            _contentRepo.Delete(file.ContentLink, false, AccessLevel.NoAccess);
            return Task.FromResult(0);
        }

        private ContentFolder GetGroupFolder(IEnumerable<string> fileGroup)
        {
            var rootReference = SiteDefinition.Current.GlobalAssetsRoot;
            ContentFolder fileGroupFolder = null;
            foreach (var groupName in fileGroup)
            {
                //if(groupName.Equals("globalassets", StringComparison.OrdinalIgnoreCase)) continue;
                fileGroupFolder = _contentRepo
                    .GetChildren<ContentFolder>(rootReference)
                    .FirstOrDefault(f => f.Name == groupName);

                //create filegroup folder if not exists 
                if (fileGroupFolder == null)
                {
                    fileGroupFolder = _contentRepo.GetDefault<ContentFolder>(rootReference);
                    fileGroupFolder.Name = groupName;
                    var folderRef = _contentRepo.Save(fileGroupFolder, SaveAction.Publish, AccessLevel.NoAccess);
                    fileGroupFolder = _contentRepo.Get<ContentFolder>(folderRef);
                }

                rootReference = fileGroupFolder.ContentLink;
            }

            return fileGroupFolder;
        }
    }
}
