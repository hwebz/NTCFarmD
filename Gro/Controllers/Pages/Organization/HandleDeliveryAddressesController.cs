using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages.Organization;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.Organization;

namespace Gro.Controllers.Pages.Organization
{
    //[OwnerCustomer]
    public class HandleDeliveryAddressesController : SiteControllerBase<HandleDeliveryAddressPage>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ISecurityRepository _securityRepository;

        public HandleDeliveryAddressesController(
            ISecurityRepository securityRepository,
            IUserManagementService userManager,
            IOrganizationRepository organizationRepository) : base(userManager)
        {
            _securityRepository = securityRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<ActionResult> Index(HandleDeliveryAddressPage currentPage)
        {
            var viewModel = await GetViewModel(currentPage);
            var hasUpdateParam = Request.QueryString["hasUpdated"] ?? string.Empty;
            var hasDeltedParam = Request.QueryString["hasDeleted"] ?? string.Empty;
            var hasAddedParam = Request.QueryString["hasAdded"] ?? string.Empty;
            bool hasUpdated;
            bool hasDeleted;
            bool hasAdded;
            if (bool.TryParse(hasUpdateParam, out hasUpdated))
            {
                viewModel.HasBeenUpdated = true;
                viewModel.UpdateSuccess = hasUpdated;
            }

            if (bool.TryParse(hasDeltedParam, out hasDeleted))
            {
                viewModel.HasBeenDeleted = true;
                viewModel.UpdateSuccess = hasDeleted;
            }
            if (!bool.TryParse(hasAddedParam, out hasAdded))
            {
                return View("~/Views/Organization/HandleDeliveryAddresses.cshtml", viewModel);
            }

            viewModel.HasBeenAdded = true;
            viewModel.UpdateSuccess = hasAdded;
            return View("~/Views/Organization/HandleDeliveryAddresses.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(HandleDeliveryAddressPage currentPage, string addressNumber, string canDelete)
        {
            return View("~/Views/Organization/HandleDeliveryAddresses.cshtml",
                await GetViewModel(currentPage, addressNumber, canDelete));
        }

        private async Task<HandleDeliveryAddressesViewModel> GetViewModel(HandleDeliveryAddressPage currentPage, string addressNumber = "", string canDelete = "")
        {
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
            var viewModel = new HandleDeliveryAddressesViewModel(currentPage);
            if (customerId <= 0) return viewModel;

            var customerAdresses = await _organizationRepository.GetCustomersDeliveryAddressesAsync(customerId);
            customerAdresses = customerAdresses ?? new CustomerDeliveryAddress[0];

            if (string.IsNullOrEmpty(addressNumber))
            {
                viewModel.DeliveryAddresses = await PopulateListAddressModel(customerId, customerAdresses);
            }
            else
            {
                var address =
                    customerAdresses?.FirstOrDefault(
                        i => i.AddressNumber != null && i.AddressNumber.Equals(addressNumber));

                if (address == null)
                {
                    viewModel.DeliveryAddresses = await PopulateListAddressModel(customerId, customerAdresses);
                    return viewModel;
                }

                bool canDeleteValue;
                var listNotifcationReceivers = await _organizationRepository.GetDeliveryAddressReceiversAsync(customerId, addressNumber);
                var listAllReceivers = await _securityRepository.GetUsersForCustomerAsync(customerId);
                viewModel.EditingDeliveryAddress = new SingleDeliveryAddressViewModel(true)
                {
                    DeliveryAddress = OrganizationViewHelper.PopulateAdressModels(address, listNotifcationReceivers, listAllReceivers),
                    Instruction = currentPage.InstructionForEditting,
                    CustomerName = activeCustomer != null ? activeCustomer.CustomerName : string.Empty,
                    CanDelete = bool.TryParse(canDelete, out canDeleteValue) && canDeleteValue
                };
            }
            return viewModel;
        }

        private async Task<IList<AddressViewModel>> PopulateListAddressModel(int customerId, CustomerDeliveryAddress[] customerAdresses)
        {
            var result = new List<AddressViewModel>();
            if (customerAdresses.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var address in customerAdresses)
            {
                var listNotifcationReceivers = await _organizationRepository.GetDeliveryAddressReceiversAsync(customerId,
                    address.AddressNumber);
                result.Add(OrganizationViewHelper.PopulateAdressModels(address, listNotifcationReceivers, null));
            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAddress(HandleDeliveryAddressPage currentPage, AddressViewModel model)
        {
            model.Silos = OrganizationViewHelper.PopulateSilos(model.Silos);
            var customerDeliveryAddress = OrganizationViewHelper.PopulateCustomerDeliveryAddress(model);
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
            if (customerId == 0)
            {
                return RedirectToAction("Index", new { hasUpdated = false });
            }
            var result = _organizationRepository.UpdateDeliveryAddress(customerDeliveryAddress);

            // update list notifcation receivers
            var listPreviousChoosen =
                await _organizationRepository.GetDeliveryAddressReceiversAsync(customerId, model.AddressNumber);
            var listChoosenIds = OrganizationViewHelper.GetListChoosenReceiverIds(model.NotificationReceiverModels);
            var listRemovedIds = GetListRemovedIds(listPreviousChoosen, listChoosenIds);
            var listNewAddedIds = GetListAddedIds(listChoosenIds, listPreviousChoosen);
            if (!listRemovedIds.IsNullOrEmpty())
            {
                await _organizationRepository.DeleteDeliveryAddressReceiversAsync(customerId, listRemovedIds.ToArray(),
                    model.AddressNumber);
            }
            if (!listNewAddedIds.IsNullOrEmpty())
            {
                await _organizationRepository.CreateDeliveryAddressReceiversAsync(customerId, listNewAddedIds.ToArray(),
                    model.AddressNumber);
            }
            return RedirectToAction("Index", new { hasUpdated = result });
        }

        private static List<int> GetListAddedIds(List<int> listChoosenIds, DeliveryReceiver[] listPreviousChoosen)
        {
            if (listPreviousChoosen.IsNullOrEmpty() || listPreviousChoosen.IsNullOrEmpty())
            {
                return listChoosenIds;
            }

            return listChoosenIds.Where(i => listPreviousChoosen.All(item => item.UserId != i)).Select(i => i).ToList();
        }

        private static List<int> GetListRemovedIds(DeliveryReceiver[] listPreviousChoosen, List<int> listChoosenIds)
        {
            var listRemovedIds = new List<int>();
            if (listPreviousChoosen.IsNullOrEmpty()) return listRemovedIds;

            if (listChoosenIds.IsNullOrEmpty())
            {
                listRemovedIds = listPreviousChoosen.Select(i => i.UserId).ToList();
            }
            listRemovedIds.AddRange(from item in listPreviousChoosen
                                    where listChoosenIds.All(i => i != item.UserId)
                                    select item.UserId);
            return listRemovedIds;
        }

        [Route("api/handle-delivery-address/delete/{addressNumber}/{handlePageId}")]
        public ActionResult DeleteAddress(string addressNumber, string handlePageId)
        {
            // handle delete
            if (string.IsNullOrEmpty(addressNumber) || string.IsNullOrEmpty(handlePageId))
            {
                return Content("");
            }
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
            if (customerId == 0)
            {
                return Content("");
            }
            var deleteSuccess = _organizationRepository.DeleteDeliveryAddress(customerId, addressNumber);

            var urlHelper = new UrlHelper(Request.RequestContext);
            var hanlderPageUrl = urlHelper.ContentUrl(new ContentReference(handlePageId));
            hanlderPageUrl = $"{hanlderPageUrl}?hasDeleted={deleteSuccess}";
            return Content(hanlderPageUrl);
        }
    }
}
