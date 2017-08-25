using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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

namespace Gro.Business.Services.Machines
{
    public class GroMachineService : IGroMachineService
    {
        private const string CategoryListXmlFileName = "intypeepi.xml";
        private const string BrandListXmlFileName = "models.xml";
        private readonly IGroContentDataService _groContentservice;

        public GroMachineService(IGroContentDataService groContentDataService)
        {
            _groContentservice = groContentDataService;
        }

        public string GetMachineListUrl(HttpContextBase httpContext)
        {
            if (PageReference.IsNullOrEmpty(ContentReference.StartPage)) return null;
            var startPage = ContentReference.StartPage.Get<StartPage>();

            var maskinStartPage = _groContentservice.GetChildrenForVisitor<MaskinStartPage>(startPage, httpContext).FirstOrDefault();
            if (maskinStartPage == null) return null;

            var urlHelper = ServiceLocator.Current.GetInstance<UrlHelper>();
            return urlHelper.ContentUrl(maskinStartPage.ContentLink);
        }

        /// <summary>
        /// GetVirtualPath: get virtual path for machine image and documents.
        /// </summary>
        /// <param name="vppFolder"></param>
        /// <param name="nameWithExtension"></param>
        /// <returns>full virtual path for image/doucment</returns>
        public string GetVirtualPath(string vppFolder, string nameWithExtension)
        {
            var vpp = System.Web.Hosting.HostingEnvironment.VirtualPathProvider as VirtualPathNonUnifiedProvider;
            if (vpp == null)
            {
                return string.Empty;
            }
            var vppRoot = vpp.VirtualPathRoot;
            var fileVirtualPath = $"{vppRoot}{vppFolder}/{nameWithExtension}";

            return vpp.FileExists(fileVirtualPath) ? fileVirtualPath : string.Empty;
        }

        public string GetPhysicalPath(string vppFolder, string nameWithExtension)
        {
            var vpp = System.Web.Hosting.HostingEnvironment.VirtualPathProvider as VirtualPathNonUnifiedProvider;
            if (vpp == null)
            {
                return string.Empty;
            }
            var vppRoot = vpp.VirtualPathRoot;
            var fileVirtualPath = $"{vppRoot}{vppFolder}/{nameWithExtension}";

            return vpp.FileExists(fileVirtualPath) ? $"{vpp.LocalPath}{vppFolder}/{nameWithExtension}" : string.Empty;
        }

        public string GetMachineImageUrl(List<MachineImage> images)
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

        public List<MachineBrand> GetBrandListFromXml()
        {
            var doc = new XmlDocument();
            //var xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/models.xml");
            var xmlPath = GetPhysicalPath(VirtualPathConfig.XmlFolder, BrandListXmlFileName);
            if (string.IsNullOrEmpty(xmlPath))
            {
                xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/models.xml");
            }
            doc.Load(xmlPath);

            if (doc.DocumentElement == null) return new List<MachineBrand>();

            var brandList = new List<MachineBrand>();
            foreach (XmlNode brandNode in doc.DocumentElement.ChildNodes)
            {
                var modelList = new List<MachineModel>();
                foreach (XmlNode modelNode in brandNode.ChildNodes)
                {
                    if (modelNode.Name == "Object")
                    {
                        modelList.Add(new MachineModel
                        {
                            Id = modelNode.ChildNodes[1].InnerText,
                            Key = modelNode.ChildNodes[5].InnerText,
                            Name = modelNode.ChildNodes[4].InnerText
                        });
                    }
                }
                brandList.Add(new MachineBrand
                {
                    Id = brandNode.ChildNodes[1].InnerText,
                    Key = brandNode.ChildNodes[5].InnerText,
                    Name = brandNode.ChildNodes[4].InnerText,
                    ModelList = modelList
                });
            }
            return brandList;
        }

        public List<MachineCategory> GetCategoryListFromXml()
        {
            var doc = new XmlDocument();
            //var xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/intypeepi.xml");
            var xmlPath = GetPhysicalPath(VirtualPathConfig.XmlFolder, CategoryListXmlFileName);
            if (string.IsNullOrEmpty(xmlPath))
            {
                xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/intypeepi.xml");
            }
            doc.Load(xmlPath);

            if (doc.DocumentElement == null) return new List<MachineCategory>();

            return (from XmlNode categoryNode in doc.DocumentElement.ChildNodes
                select new MachineCategory
                {
                    Id = categoryNode.ChildNodes[1].InnerText,
                    Key = categoryNode.ChildNodes[5].InnerText,
                    Name = categoryNode.ChildNodes[4].InnerText,
                }).ToList();
        }

        #region private members

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

        #endregion
    }
}
