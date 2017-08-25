using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Core;
using Gro.Business.Services.News;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MachinePages;
using Gro.Core.DataModels.Machine;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.Machines;
using Gro.Business.Services.Machines;
using Gro.Constants;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Controllers.Pages.Machines
{
    public class MachineDetailPageController : SiteControllerBase<MachineDetailPage>
    {
        private const string Machine_W = "Maskin_w";

        private readonly MediaConfig _mediaConfig;
        private readonly IMachineRepository _machineRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IOrganizationUserRepository _orgUserRepo;
        private readonly IGroMachineService _groMachineService;

        public MachineDetailPageController(IMachineRepository machineRepository,
            IOrganizationUserRepository orgUserRepo,
            IUserManagementService userManager,
            IFileRepository fileRepository,
            MediaConfig mediaConfig,
            IGroContentDataService groContentDataService,
            IGroMachineService groMachineService) : base(userManager)
        {
            _orgUserRepo = orgUserRepo;
            _machineRepository = machineRepository;
            _fileRepository = fileRepository;
            _mediaConfig = mediaConfig;
            _groMachineService = groMachineService;
        }

        [CustomerRole]
        public async Task<ActionResult> Index(MachineDetailPage currentPage, string maid)
        {
            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null)
            {
                return View("~/Views/Machine/MachineDetail/Index.cshtml", new MachineDetailPageViewModel(currentPage) { Machine = new Machine() });
            }
            var userRoles = await _orgUserRepo.GetUserCustomerRolesAsync(SiteUser.UserName, organization.CustomerNo);

            var machine = await _machineRepository.GetDetailMachineById(maid);
            var bookServiceUrl = ContentExtensions.GetPageUnderSettingUrl(x => x.BookServicePage);

            var model = new MachineDetailPageViewModel(currentPage)
            {
                Machine = machine ?? new Machine(),
                IsHasMachineWRight = userRoles?.Any(r => r.RoleName.Equals(Machine_W)) ?? false,
                UrlBookService = bookServiceUrl
            };

            var machineOwnerPic = await _machineRepository.GetMachinePicUrl(organization.CustomerId, maid);

            model.IsHasMachineOwnerPicture = !string.IsNullOrEmpty(machineOwnerPic?.PictureUrl);

            model.Machine.ImageUrl = machineOwnerPic != null
                ? machineOwnerPic.PictureUrl
                : _groMachineService.GetMachineImageUrl(model.Machine.Images);

            foreach (var doc in model.Machine.Documents)
            {
                if (!string.IsNullOrEmpty(doc.Url)) continue;

                var fileExtension = Path.GetExtension(doc.FileName);
                var fullNameWithExtension = $"{doc.Id}{fileExtension}";
                doc.Url = _groMachineService.GetVirtualPath(VirtualPathConfig.DocumentsFolder, fullNameWithExtension);
            }

            return View("~/Views/Machine/MachineDetail/Index.cshtml", model);
        }

        [Route("api/machine/machine-upload-avatar")]
        [HttpPost]
        public async Task<JsonResult> UpdateMachinePicture(string machineId)
        {
            if (SiteUser == null || !Request.Files.AllKeys.Any()) return Json(new {success = false});

            // Get the uploaded image from the Files collection
            var httpPostedFile = Request.Files[_mediaConfig.HttpPostedFileKey];

            if (httpPostedFile == null || !httpPostedFile.IsImage()) return Json(new {success = false});
            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null) return Json(new {success = false});

            var fileExtension = Path.GetExtension(httpPostedFile.FileName);
            var machineOwnerPic = await _machineRepository.GetMachinePicUrl(organization.CustomerId, machineId);
            if (machineOwnerPic != null)
            {
                await _machineRepository.DeleteMachinePicUrl(machineOwnerPic.Id);
                await _fileRepository.DeleteAsync(machineOwnerPic.PictureUrl);
            }

            var url = await _fileRepository.SaveAsync(httpPostedFile.InputStream, fileExtension, $"{_mediaConfig.MachineFolder}/Images", SiteUser.UserName);
            if (url == null) return Json(new {success = false});
            var result = await _machineRepository.CreateMachinePicUrl(organization.CustomerId, machineId, url);

            return Json(result ? new { success = true } : new { success = false });
        }

        [Route("api/machine/machine-delete-avatar")]
        [HttpPost]
        public async Task<JsonResult> DeleteMachinePicture(string machineId)
        {
            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null) return Json(new {success = false});

            var machinePic = await _machineRepository.GetMachinePicUrl(organization.CustomerId, machineId);
            if (machinePic == null) return Json(new {success = false});

            await _fileRepository.DeleteAsync(machinePic.PictureUrl);
            var result = await _machineRepository.DeleteMachinePicUrl(machinePic.Id);
            if (!result) return Json(new { success = false });

            var machine = await _machineRepository.GetDetailMachineById(machineId) ?? new Machine();
            var machineImageUrl = _groMachineService.GetMachineImageUrl(machine.Images);
            return Json(new { success = true, imageUrl = machineImageUrl });
        }

        [HttpPost]
        [CustomerRole("Maskin_w")]
        public async Task<ActionResult> RemoveMachineById(MachineDetailPage currentPage, string machineId)
        {
            var result = await _machineRepository.RemoveMachine(machineId);
            if (result)
            {
                var machineListUrl = _groMachineService.GetMachineListUrl(HttpContext);
                if (!string.IsNullOrEmpty(machineListUrl))
                {
                    return Redirect(machineListUrl);
                }
                return RedirectToAction("Index", new { node = ContentReference.StartPage });
            }
            TempData["DeleteFailed"] = "True";
            return RedirectToAction("Index", new { node = currentPage.ContentLink, maid = machineId });
        }
    }
}
