using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Gro.Business.Services.Machines;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.DataModels.Machine;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.ViewModels.Pages.Machines;

namespace Gro.Controllers.Pages.Machines
{
    public class MaskinStartPageController : SiteControllerBase<MaskinStartPage>
    {
        // GET: MachineList
        private readonly IMachineRepository _machineRepository;

        private readonly IGroMachineService _groMachineService;

        public MaskinStartPageController(IMachineRepository machineRepository,
            IUserManagementService usersManagementService,
            IGroMachineService groMachineService,
            MediaConfig mediaConfig) : base(usersManagementService)
        {
            _machineRepository = machineRepository;
            _groMachineService = groMachineService;
        }

        public async Task<ActionResult> Index(MaskinStartPage currentPage)
        {
            var organization = UserManager.GetActiveCustomer(HttpContext);
            if (organization == null)
            {
                return View("Index", new MaskinStartPageViewModel(currentPage));
            }

            var htmlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            var detailUrl = htmlHelper.ContentUrl(currentPage.MachineDetailPage);
            var machines = _machineRepository.GetAllMachinesForCustomerId(organization.CustomerNo);
#if DEBUG
            machines = _machineRepository.GetAllMachinesForCustomerId("19036719");
#endif
            //#if !DEBUG
            //              machines = _machineRepository.GetAllMachinesForCustomerId(organization.CustomerNo);
            //#endif

            var machineOwnerPictures = await _machineRepository.FindMachinePictures(organization.CustomerId);
            if (machines != null)
            {
                var machineOwnerPicDic = machineOwnerPictures?.GroupBy(s => s.PictureId).ToDictionary(s => s.Key, v => v.FirstOrDefault()) ??
                                         new Dictionary<string, OrganisationPicture>();
                foreach (var machine in machines)
                {
                    var machineOwnerPic = machineOwnerPicDic.ContainsKey(machine.Id) ? machineOwnerPicDic[machine.Id] : null;

                    //var machineImage = machine.Images != null && machine.Images.Count > 0
                    //    ? $"{machine.Images[0].Id}.jpg"
                    //    : string.Empty;

                    //var categoryImage = _machineRepository.GetMachineCategoryImage(machine.Group.Id);

                    //machine.ImageUrl = !string.IsNullOrEmpty(machineOwnerPic?.PictureUrl) ? machineOwnerPic.PictureUrl :
                    //                   !string.IsNullOrEmpty(machineImage) ?
                    //                  MachineHelper.GetVirtualPath(VirtualPathConfig.ImageFolder, machineImage) : categoryImage;
                    machine.ImageUrl = !string.IsNullOrEmpty(machineOwnerPic?.PictureUrl)
                        ? machineOwnerPic.PictureUrl
                        : _groMachineService.GetMachineImageUrl(machine.Images);
                }
            }

            var machineListPage = new MaskinStartPageViewModel(currentPage)
            {
                DetailMachineUrl = detailUrl,
                ListMachine = machines ?? new List<Machine>()
            };

            return View("Index", machineListPage);
        }
    }
}