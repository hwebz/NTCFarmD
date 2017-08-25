using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Business.Services.Machines;
using Gro.Business.Services.News;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.MachinePages;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.Machine;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Machines;

namespace Gro.Controllers.Pages.Machines
{
    [RoutePrefix("api/add-machine")]
    public class MachineAddController : SiteControllerBase<MachineAddPage>
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IGroMachineService _groMachineService;

        public MachineAddController(
            IUserManagementService userManager,
            IMachineRepository machineRepository,
            IGroContentDataService groContentDataService,
            IGroMachineService groMachineService) : base(userManager)
        {
            _machineRepository = machineRepository;
            _groMachineService = groMachineService;
        }

        public ActionResult Index(MachineAddPage currentPage)
        {
            var machinePage = new MachineAddPageViewModel(currentPage)
            {
                CategoryList = _groMachineService.GetCategoryListFromXml(),
                BrandList = _groMachineService.GetBrandListFromXml(),
                UrlMaskinStarPage = _groMachineService.GetMachineListUrl(HttpContext)
            };

            return View("~/Views/Machine/MachineAdd/Index.cshtml", machinePage);
        }

        [HttpPost]
        [CustomerRole("Maskin_w")]
        public ActionResult Index(MachineAddPage currentPage, FormCollection collection)
        {
            var categoryName = collection["machineCategory"];
            var brand = collection["machineBrand"];
            var model = collection["machineModel"];
            var brandDescription = collection["branch-input"];
            var modelDescription = collection["model-input"];
            var otherCateName = collection["other-cate-name"];

            var supplier = UserManager.GetActiveCustomer(HttpContext);

            var machine = new Machine
            {
                Brand = new MachineBrand { Key = brand == "Annan" ? "Other" : brand },
                Model = new MachineModel { Key = model == "Annan" ? "Other" : model },
                DescriptionBrand = brandDescription,
                DescriptionModel = modelDescription,
                OwnerNumber = supplier != null ? supplier.CustomerNo : string.Empty,
                IndvidualType = categoryName.Equals("Other") ? otherCateName : categoryName,
            };

            var isSuccess = _machineRepository.AddNewMachine(machine);
            if (isSuccess)
            {
                var machinePageReference = ContentExtensions.GetSettingsPage()?.MachineStartPage;
                return RedirectToAction("Index", new { node = machinePageReference });
            }

            var vm = new MachineAddPageViewModel(currentPage)
            {
                CategoryList = _groMachineService.GetCategoryListFromXml(),
                BrandList = _groMachineService.GetBrandListFromXml()
            };
            return View("~/Views/Machine/MachineAdd/Index.cshtml", vm);
        }

        [Route("get-models")]
        [CustomerRole("Maskin_w")]
        public JsonResult GetMachineModel(string id)
        {
            var brandList = _groMachineService.GetBrandListFromXml();
            var brand = brandList.FirstOrDefault(x => x.Key == id);
            if (brand == null) return Json(new { machineModels = "" }, JsonRequestBehavior.AllowGet);

            var models = brand.ModelList.ToArray();
            var jsModels = models.Select(x => new
            {
                id = x.Id,
                key = x.Key,
                name = x.Name
            });
            return Json(new { machineModels = jsModels }, JsonRequestBehavior.AllowGet);
        }

        [Route("get-machine-models")]
        [CustomerRole("Maskin_w")]
        public ActionResult GetMachineModels(string brandId, string modelName = "")
        {
            var brandList = _groMachineService.GetBrandListFromXml();
            var brand = brandList.FirstOrDefault(x => x.Key == brandId);
            var listModels = new List<MachineModel>();
            if (brand == null) return PartialView("~/Views/Machine/MachineAdd/ModelLists.cshtml", new MachineAddModelList()
            {
                ListModels = listModels
            });

            var models = brand.ModelList.ToArray();
            listModels.AddRange(models);
            var selectedModel = listModels.FirstOrDefault(x => x.Name.Equals(modelName));

            return PartialView("~/Views/Machine/MachineAdd/ModelLists.cshtml", new MachineAddModelList()
            {
                ListModels = listModels,
                SelectedModel = selectedModel
            });
        }

        [Route("check-register-number")]
        [CustomerRole("Maskin_w")]
        public async Task<JsonResult> CheckRegisterNumber(string registerNumber)
        {
            var machine = await _machineRepository.GetDetailMachineByRegNumber(registerNumber);
            if (machine != null)
            {
                return Json(new
                {
                    id = machine.Id,
                    brand = machine.Brand,
                    descBrand = machine.DescriptionBrand,
                    model = machine.Model,
                    descModel = machine.ModelDescription,
                    category = machine.Group,
                    serialNumber = machine.SerialNumber
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}
