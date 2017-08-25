using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MyProfile;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.MyProfile;

namespace Gro.Controllers.Pages.MyProfile
{
    public class MyAccountController : SiteControllerBase<MyAccountPage>
    {
        private readonly MediaConfig _mediaConfig;
        private readonly ISecurityRepository _securityRepository;
        private readonly IOrganizationRepository _organizationRepo;
        private readonly IFileRepository _fileRepository;

        public MyAccountController(
            ISecurityRepository securityRepository,
            IUserManagementService userManager,
            IOrganizationRepository organizationRepository,
            IFileRepository fileRepository,
            MediaConfig mediaConfig) : base(userManager)
        {
            _securityRepository = securityRepository;
            _organizationRepo = organizationRepository;
            _mediaConfig = mediaConfig;
            _fileRepository = fileRepository;
        }

        public ActionResult Index(MyAccountPage currentPage)
        {
            if (SiteUser == null)
            {
                return View("~/Views/MyProfile/MyAccount.cshtml", new MyAccountPageViewModel(currentPage));
            }
            var organization = UserManager.GetActiveCustomer(HttpContext);

            var model = new MyAccountPageViewModel(currentPage)
            {
                CurrentOrganization = organization,
                UserProfilePictureUrl = SiteUser?.ProfilePicUrl,
                CompanyProfilePictureUrl = organization?.ProfilePicUrl
            };

            return View("~/Views/MyProfile/MyAccount.cshtml", model);
        }

        [Route("api/profile/user-upload-avatar")]
        [HttpPost]
        public async Task<JsonResult> UpdateProfilePicture()
        {
            if (SiteUser == null || Request.Files.Count == 0) return ApiResult(false);

            // Get the uploaded image from the Files collection
            var httpPostedFile = Request.Files[_mediaConfig.HttpPostedFileKey];
            if (httpPostedFile == null || !httpPostedFile.IsImage()) return ApiResult(false);

            var fileExtension = Path.GetExtension(httpPostedFile.FileName);
            await _fileRepository.DeleteAsync(SiteUser.ProfilePicUrl);
            var url = await _fileRepository.SaveAsync(httpPostedFile.InputStream, fileExtension, _mediaConfig.UserFolder, SiteUser.UserName);

            //save the url
            var saveResult = await _securityRepository.SaveUserPictureUrl(SiteUser.UserId, url);
            if (!saveResult) ApiResult(false);

            RefreshProfilePicture(url);
            return ApiResult(true);
        }

        [Route("api/profile/user-delete-avatar")]
        [HttpPost]
        public async Task<JsonResult> DeleteProfilePicture()
        {
            if (SiteUser == null) return ApiResult(false);
            await _fileRepository.DeleteAsync(SiteUser.ProfilePicUrl);

            var result = await _securityRepository.DeleteUserPictureUrl(SiteUser.UserId);
            if (result)
            {
                RefreshProfilePicture(string.Empty);
            }

            return Json(new { success = result, imageUrl = "/Static/images/user-avatar__default.jpg" }, JsonRequestBehavior.AllowGet);
        }

        [Route("api/profile/company-upload-avatar")]
        [HttpPost]
        public async Task<JsonResult> UpdateCompanyPicture()
        {
            if (SiteUser == null || Request.Files.Count == 0) return ApiResult(false);

            // Get the uploaded image from the Files collection
            var httpPostedFile = Request.Files[_mediaConfig.HttpPostedFileKey];
            if (httpPostedFile == null || !httpPostedFile.IsImage()) return ApiResult(false);

            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null) return ApiResult(false);

            //var fileId = MediaHelper.UploadFile(httpPostedFile, ParentFolder, organization.CustomerName, organization.ProfilePicURL);
            var fileExtension = Path.GetExtension(httpPostedFile.FileName);
            await _fileRepository.DeleteAsync(organization.ProfilePicUrl);
            var url = await _fileRepository.SaveAsync(httpPostedFile.InputStream, fileExtension, _mediaConfig.CustomerFolder, SiteUser.UserName);

            var saveResult = await _organizationRepo.SaveOrganizationPictureUrlAsync(organization.CustomerId, url);

            if (saveResult)
            {
                RefreshCustomers();
            }
            return ApiResult(saveResult);
        }

        [Route("api/profile/company-delete-avatar")]
        [HttpPost]
        public async Task<JsonResult> DeleteCompanyPicture()
        {
            if (SiteUser == null) return ApiResult(false);
            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null) return ApiResult(false);

            await _fileRepository.DeleteAsync(organization.ProfilePicUrl);
            var result = await _organizationRepo.DeleteOrganizationPictureUrl(organization.CustomerId);
            if (result)
            {
                RefreshCustomers();
            }

            return Json(new { success = result, imageUrl = "/Static/images/location__default.jpg" }, JsonRequestBehavior.AllowGet);
        }

        private void RefreshProfilePicture(string url)
        {
            SiteUser.ProfilePicUrl = url;
            this.SetUserSession(SiteUser);
        }

        private void RefreshCustomers() => UserManager.GetActiveCustomer(HttpContext, true);

        private JsonResult ApiResult(bool success) => Json(new { success = success }, JsonRequestBehavior.AllowGet);
    }
}
