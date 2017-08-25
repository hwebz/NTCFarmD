using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.SessionState;
using EPiServer.Framework.DataAnnotations;
using Gro.Business.Services.Users;
using Gro.Core.ContentTypes.Pages;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.Interfaces;
using Gro.ViewModels.Pages.Messages;
using NuGet;

namespace Gro.Controllers.Pages
{
    [SessionState(SessionStateBehavior.Disabled)]
    [TemplateDescriptor(Inherited = true)]
    public class DeliveryMessagesPageController : SiteControllerBase<DeliveryMessagesPage>
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IUserManagementService _userManger;

        public DeliveryMessagesPageController(IUserMessageRepository userMessageRepository, IUserManagementService userManagement)
        {
            _userMessageRepository = userMessageRepository;
            _userManger = userManagement;
        }

        public ViewResult Index(DeliveryMessagesPage currentPage)
        {
            var activeCustomer = _userManger.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
#if DEBUG
            customerId = 16;
#endif
            if (customerId <= 0) return View("Index", new PlannedDeliveryPageViewModel(currentPage) { PlannedDeliveriesList = new PlannedDeliveries[0] });

            var plannedDeliveriesList = _userMessageRepository.GetPlannedDeliveriesList(customerId, 0);
            plannedDeliveriesList = plannedDeliveriesList ?? new PlannedDeliveries[0];

            var firstDelivery = plannedDeliveriesList.Length > 0 ? plannedDeliveriesList[0] : null;
            var lastUpdateDate = firstDelivery?.LastUpdated ?? DateTime.Now;

            var model = new PlannedDeliveryPageViewModel(currentPage)
            {
                ActiveCustomer = activeCustomer,
                PlannedDeliveriesList = plannedDeliveriesList,
                LastUpdated = lastUpdateDate
            };
            return View("Index", model);
        }
    }
}
