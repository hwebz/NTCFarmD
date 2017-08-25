using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Hosting;
using EPiServer.Web.Mvc.Html;
using Gro.Business.Services.News;
using Gro.Constants;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.Machine;
using Gro.Infrastructure.Data;

namespace Gro.Helpers
{
    public static class MachineHelper
    {
        public static string GetMachineListUrl(IGroContentDataService groContentDataService, HttpContextBase httpContext)
        {
            if (PageReference.IsNullOrEmpty(ContentReference.StartPage)) return null;
            var startPage = ContentReference.StartPage.Get<StartPage>();

            var maskinStartPage = groContentDataService.GetChildrenForVisitor<MaskinStartPage>(startPage, httpContext).FirstOrDefault();
            if (maskinStartPage == null) return null;

            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            return urlHelper.ContentUrl(maskinStartPage.ContentLink);
        }

        /// <summary>
        /// GetVirtualPath: get virtual path for machine image and documents.
        /// </summary>
        /// <returns>full virtual path for image/doucment</returns>
        public static string GetVirtualPath(string vppFolder, string nameWithExtension)
        {
            var vpp = System.Web.Hosting.HostingEnvironment.VirtualPathProvider as VirtualPathNonUnifiedProvider;
            if (vpp == null) return string.Empty;

            var vppRoot = vpp.VirtualPathRoot;
            var fileVirtualPath = $"{vppRoot}{vppFolder}/{nameWithExtension}";

            return vpp.FileExists(fileVirtualPath) ? fileVirtualPath : string.Empty;
        }

        public static string GetPhysicalPath(string vppFolder, string nameWithExtension)
        {
            var vpp = System.Web.Hosting.HostingEnvironment.VirtualPathProvider as VirtualPathNonUnifiedProvider;
            if (vpp == null)
            {
                return string.Empty;
            }
            var vppRoot = vpp.VirtualPathRoot;
            var fileVirtualPath = $"{vppRoot}{vppFolder}/{nameWithExtension}";

            return vpp.FileExists(fileVirtualPath) ? $"{vpp.LocalPath}{vppFolder}{nameWithExtension}" : string.Empty;
        }

        public static string GetMachineImageUrl(List<MachineImage> images)
        {
            if (images.IsNullOrEmpty()) return string.Empty;
            var vpp = System.Web.Hosting.HostingEnvironment.VirtualPathProvider as VirtualPathNonUnifiedProvider;
            if (vpp == null) return string.Empty;

            var imgProductVirutalUrl = GetImageReplyingFromVpp(images, MachineImageType.ProductImage, vpp);

            if (!string.IsNullOrEmpty(imgProductVirutalUrl))
            {
                return imgProductVirutalUrl;
            }
            var imgCategoryVirtualUrl = GetImageReplyingFromVpp(images, MachineImageType.CategoryImage, vpp);
            return imgCategoryVirtualUrl;
        }

        private static string GetImageReplyingFromVpp(IEnumerable<MachineImage> images, string imageType, VirtualPathNonUnifiedProvider vpp)
        {
            var imgs = images.Where(i => i.ImageType != null && i.ImageType.Equals(imageType));
            foreach (var img in imgs)
            {
                var imagVirtualPath = GenerateVppUrl(img, vpp.VirtualPathRoot);
                if (!string.IsNullOrEmpty(imagVirtualPath) && vpp.FileExists(imagVirtualPath)) return imagVirtualPath;
            }
            return string.Empty;
        }

        private static string GenerateVppUrl(MachineMedia img, string vppRoot)
        {
            return string.IsNullOrEmpty(img?.Id) ? string.Empty : $"{vppRoot}{VirtualPathConfig.ImageFolder}/{img.Id}.jpg";
        }
    }
}
