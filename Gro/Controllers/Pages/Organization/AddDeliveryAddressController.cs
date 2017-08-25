using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.ContentTypes.Pages.Organization;
using System.Web.Mvc;
using Castle.Core.Internal;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.Organization;
using Gro.Core.DataModels.Security;
using Gro.Core.Interfaces;
using Gro.Helpers;
using Gro.ViewModels.Pages.Organization;

namespace Gro.Controllers.Pages.Organization
{
    public class AddDeliveryAddressController : SiteControllerBase<AddDeliveryAddressPage>
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public AddDeliveryAddressController(
            ISecurityRepository securityRepository,
            IUserManagementService userManager,
            IOrganizationRepository organizationRepository) : base(userManager)
        {
            _securityRepository = securityRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<ActionResult> Index(AddDeliveryAddressPage currentPage)
        {
            var viewModel = new DeliveryAddressViewModel(currentPage)
            {
                DeliveryAddressModel = new SingleDeliveryAddressViewModel(false)
            };

            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;

            if (customerId > 0)
            {
                var allReceivers = await _securityRepository.GetUsersForCustomerAsync(customerId);
                viewModel.DeliveryAddressModel.DeliveryAddress.Receivers = allReceivers.IsNullOrEmpty()
                    ? new List<User>()
                    : allReceivers.ToList();
            }
            viewModel.DeliveryAddressModel.CustomerName = activeCustomer != null
                ? activeCustomer.CustomerName
                : string.Empty;
            return View("~/Views/Organization/DeliveryAdresses/AddDeliveryAddress.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(AddDeliveryAddressPage currentPage, AddressViewModel model)
        {
            var activeCustomer = UserManager.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
            var addedSuccess = false;
            if (customerId > 0)
            {
                model.Silos = OrganizationViewHelper.PopulateSilos(model.Silos);
                addedSuccess = await AddNewDeliveryAddress(customerId, model);
            }

            var targetPageUrl = ContentExtensions.GetPageUnderSettingUrl(s => s.HandleAddressPage);
            targetPageUrl = string.IsNullOrEmpty(targetPageUrl) ? ContentExtensions.GetStartPageUrl() : $"{targetPageUrl}?hasAdded={addedSuccess}";
            return Redirect(targetPageUrl);
        }

        private async Task<bool> AddNewDeliveryAddress(int customerId, AddressViewModel model)
        {
            var silos = model.Silos.IsNullOrEmpty() ? new SiloItem[0] : model.Silos.ToArray();
            var addedItem = await _organizationRepository.CreateDeliveryAddressAsync(customerId, model.AdressStreet, model.ZipCode, model.Place,
                model.PhoneNumber, model.MobileNumber, model.Latitude, model.Longitude, model.Direction, silos);

            return addedItem != null;
        }
    }
}
