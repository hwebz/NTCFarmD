using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Gro.Business.Services.Users;
using Gro.Constants;
using Gro.Core.ContentTypes.Blocks;
using Gro.Core.ContentTypes.Utils;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.Interfaces;
using Gro.ViewModels.Blocks;

namespace Gro.Controllers.Blocks
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcPartialController,
        AvailableWithoutTag = true,
        Tags = new[] { ColumnLayout.OneColumnTag, ColumnLayout.FooterColumnTag })]
    public class UpcomingDeliveriesBlockController : BlockController<UpcomingDeliveriesBlock>
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly IUserManagementService _userManger;

        public UpcomingDeliveriesBlockController(IUserMessageRepository userMessageRepository, IUserManagementService userManger)
        {
            _userMessageRepository = userMessageRepository;
            _userManger = userManger;
        }

        public override ActionResult Index(UpcomingDeliveriesBlock currentBlock)
        {
            var viewModel = new UpcomingDeliveriesBlockViewModel
            {
                LastUpdated = $"{DateTime.Now:yyyy-MM-dd} 05:00"
            };
            var activeCustomer = _userManger.GetActiveCustomer(HttpContext);
            var customerId = activeCustomer?.CustomerId ?? 0;
#if DEBUG
            customerId = 16;
#endif
            if (customerId <= 0 || currentBlock.MaximumOfDeliveyItems <= 0) return PartialView("Index", viewModel);

            var deliveryInformation = _userMessageRepository.GetDeliverySummary(customerId,
                currentBlock.MaximumOfDeliveyItems);

            if (deliveryInformation == null) return PartialView("Index", viewModel);

            var deliveriesToCustomer = ReOrderDeliveries(deliveryInformation.ToCustomer);
            var deliveriesFromCustomer = ReOrderDeliveries(deliveryInformation.FromCustomer);
            var lastUpdated = deliveryInformation.LastUpdated.Year > 1 ? deliveryInformation.LastUpdated : DateTime.Now;

            viewModel.LastUpdated = $"{lastUpdated:yyyy-MM-dd} 05:00";
            viewModel.DeliveriesToCustomer = PopulateDeliveryCards(deliveriesToCustomer);
            viewModel.DeliveriesFromCustomer = PopulateDeliveryCards(deliveriesFromCustomer);
            viewModel.DeliveryPlanPage = GetPLannedDeliveryPage();
            return PartialView("Index", viewModel);
        }

        private ContentReference GetPLannedDeliveryPage()
        {
            var settingPage = ContentExtensions.GetSettingsPage();
            return settingPage?.PlannedDeliveryPage;
        }

        private static MessageExtended[] ReOrderDeliveries(MessageExtended[] deliveries)
        {
            if (deliveries == null || !deliveries.Any())
            {
                return deliveries;
            }

            var deliveriesDelayReceived = new List<MessageExtended>();
            var deliveriesForFuture = new List<MessageExtended>();
            var result = new List<MessageExtended>();
            foreach (var delivery in deliveries)
            {
                if (DeliveryHasDelayedToReceived(delivery))
                {
                    deliveriesDelayReceived.Add(delivery);
                }
                else
                {
                    deliveriesForFuture.Add(delivery);
                }
            }
            result.AddRange(deliveriesDelayReceived);
            result.AddRange(deliveriesForFuture);

            return result.ToArray();
        }

        private static bool DeliveryHasDelayedToReceived(MessageExtended delivery) => delivery.DeliveryDatePassed;


        /// <summary>
        ///     Converting MessageExtended to DeliveryCard which will use as carousel item.
        /// </summary>
        /// <param name="deliveryMessages"></param>
        /// <returns></returns>
        private static List<DeliveryCard> PopulateDeliveryCards(MessageExtended[] deliveryMessages)
        {
            if (deliveryMessages.IsNullOrEmpty())
            {
                return new List<DeliveryCard>();
            }
            return deliveryMessages.Select(item => new DeliveryCard
            {
                PlannedDeliveryDate = item.PlannedDeliveryDate.HasValue ? $"{item.PlannedDeliveryDate.Value:yyyy-MM-dd}" : string.Empty,
                ItemName = item.ItemName,
                OrderNo = item.OrderNo,
                OrderLine = item.OrderLine,
                Warehouse = item.Warehouse,
                OrderQuantity = item.OrderQuantity,
                Container = item.Container,
                DeliveryDatePassed = item.DeliveryDatePassed,
                HasDelayedToReceive = DeliveryHasDelayedToReceived(item),
                CategoryIconUrl = GetCategoryImage(item.Category)
            })
                .ToList();
        }

        private static string GetCategoryImage(int category)
        {
            switch (category)
            {
                case 12:
                    return "/Static/images/spannmal.png";
                case 4:
                    return "/Static/images/odling.png";
                case 5:
                    return "/Static/images/foder.png";
                default:
                    return "/Static/images/spannmal.png";
            }
        }

        [Route("api/upcomming-deliveries/get-detail")]
        public ActionResult GetDeliveryDetail(string orderNo, string orderLine, string itemName, string orderQuantity, string planedDeliveryDate, string warehouse,
            string container, string isFromCustomer)
        {
            double orderQuantityDbl;
            int orderLineInt;
            bool isFromCustomerBool;
            var deliveryCard = new DeliveryCard
            {
                OrderNo = orderNo,
                OrderLine = int.TryParse(orderLine, out orderLineInt) ? orderLineInt : 0,
                ItemName = itemName,
                OrderQuantity = double.TryParse(orderQuantity, out orderQuantityDbl) ? orderQuantityDbl : 0,
                PlannedDeliveryDate = planedDeliveryDate,
                Warehouse = warehouse,
                IsFromCustomer = bool.TryParse(isFromCustomer, out isFromCustomerBool) && isFromCustomerBool,
                Container = container
            };
            return PartialView("DeliveryDetail", deliveryCard);
        }
    }
}
