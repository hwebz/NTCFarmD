using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.Business.Services.Users;
using Gro.Helpers;
using Gro.Business;
using Gro.ViewModels.Pages.Messages;
using Td = Gro.Constants.DeliveryItemHeader;

namespace Gro.Controllers.Apis
{
    [RoutePrefix("api/user")]
    [CustomerRole]
    public class UserMessagingController : Controller
    {
        private readonly IUserMessageSettingsRepository _settingRepository;
        private readonly IUserMessageRepository _messageRepository;
        private readonly IUserManagementService _userManager;


        public UserMessagingController(
            ISecurityRepository securityRepository,
            IUserMessageSettingsRepository settingRepository,
            IUserMessageRepository messageRepository,
            IUserManagementService userManager)
        {
            _settingRepository = settingRepository;
            _messageRepository = messageRepository;
            _userManager = userManager;
        }

        private SiteUser _siteUser;
        private SiteUser SiteUser => _siteUser ?? (_siteUser = _userManager.GetSiteUser(HttpContext));

        // GET: /api/user/message-settings
        [Route("message-settings")]
        public async Task<JsonResult> GetMessageSettings()
        {
            var customer = _userManager.GetActiveCustomer(HttpContext);

            var tabs = await _settingRepository.GetSettingDisplayAsync(SiteUser.UserId, customer.CustomerId);
            if (tabs != null) return Json(tabs, JsonRequestBehavior.AllowGet);

            Response.StatusCode = 500;
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //POST: /api/user/save-settings
        [Route("save-settings")]
        [HttpPost]
        public async Task<JsonResult> SaveMessageSettings(MessageSettings[] settings)
        {
            var customer = _userManager.GetActiveCustomer(HttpContext);
            foreach (var setting in settings)
            {
                setting.UserId = SiteUser.UserId;
                setting.CustomerOrgId = customer.CustomerId;
            }

            var saveResults = await _settingRepository.SaveSettings(settings);
            return Json(saveResults);
        }

        //POST: /api/user/get-categories-by-status
        [Route("get-categories-by-status")]
        [HttpPost]
        public async Task<JsonResult> GetCategoriesByStatus(string type)
        {
            var newCategories = await GetCategoriesByMessageStatus(SiteUser.UserId, type);
            var jsCategories = newCategories?.Select(cat => new
            {
                id = cat.Categoryid,
                name = cat.CategoryName,
                isSelected = false
            });

            return Json(new { categories = jsCategories });
        }

        //POST: /api/user/get-messages
        [Route("get-messages")]
        [HttpPost]
        public async Task<JsonResult> GetMessagesForUser(string type, int pageSize, int pageIndex, string cats, bool isReloadCategory)
        {
            var categories = cats.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Message[] messages;
            Category[] newCategories = null;
            int total;
            if (!string.IsNullOrEmpty(type) && type.Equals("starred", StringComparison.OrdinalIgnoreCase))
            {
                var starred = new[] { ((int)MessageStatus.Starred).ToString() };
                messages = await _messageRepository.GetMessagesForUser(SiteUser.UserId, pageSize, pageIndex, "all", categories, starred);

                total = await _messageRepository.CountMessages(SiteUser.UserId, "all", categories, starred);

                if (isReloadCategory)
                {
                    newCategories = await _messageRepository.GetCategoriesByStatus(SiteUser.UserId, starred);
                }
            }
            else if (!string.IsNullOrEmpty(type) && type.Equals("trash", StringComparison.OrdinalIgnoreCase))
            {
                var trash = new[] { ((int)MessageStatus.InTrash).ToString() };
                messages = await _messageRepository.GetMessagesForUser(SiteUser.UserId, pageSize, pageIndex, "all", categories, trash);

                total = await _messageRepository.CountMessages(SiteUser.UserId, "all", categories, trash);

                if (isReloadCategory)
                {
                    newCategories = await _messageRepository.GetCategoriesByStatus(SiteUser.UserId, trash);
                }
            }
            else
            {
                messages = await _messageRepository.GetMessagesForUser(SiteUser.UserId, pageSize, pageIndex, "all", categories,
                    new[] { ((int)MessageStatus.Default).ToString(), ((int)MessageStatus.Starred).ToString(), ((int)MessageStatus.Archived).ToString() });
                total = await _messageRepository.CountMessages(SiteUser.UserId, "all", categories,
                    new[] { ((int)MessageStatus.Default).ToString(), ((int)MessageStatus.Starred).ToString(), ((int)MessageStatus.Archived).ToString() });

                if (isReloadCategory)
                {
                    //var allCategories = await _messageRepository.GetAllCategories(SiteUser.UserId);
                    var allCategories = await _messageRepository.GetCategoriesForFiltering(SiteUser.UserId);
                    newCategories = allCategories?.Select(c => new Category
                    {
                        Categoryid = c.EnvTypesId,
                        CategoryName = c.EnvTypeDescription
                    }).ToArray();
                }
            }

            if (messages == null) return Json(new { });

            var jsmessages = messages.Select(msg => new
            {
                id = msg.MessageId,
                header = msg.HeadLine,
                content = msg.MsgText,
                categoryId = msg.MessageArea,
                categoryName = msg.AreaDescription,
                isUnRead = !msg.MessageRead,
                isStarred = msg.Status == (int)MessageStatus.Starred,
                isTrash = msg.Status == (int)MessageStatus.InTrash,
                isDelete = msg.Status == (int)MessageStatus.Deleted,
                receivedDate = $"{msg.SendDate:yyyy-MM-dd}"
            });

            var jsCategories = newCategories?
                .OrderBy(cat => cat.CategoryName)
                .Select(cat => new
                {
                    id = cat.Categoryid,
                    name = cat.CategoryName,
                    isSelected = false
                });


            return Json(new
            {
                messages = jsmessages,
                categories = jsCategories,
                total = total
            });
        }

        //POST: /api/user/get-message
        [Route("get-message")]
        [HttpPost]
        public async Task<JsonResult> GetMessage(int msgId)
        {
            var result = await _messageRepository.GetDetailMessage(msgId, SiteUser.UserId);
            var message = result.Keys.FirstOrDefault();
            var previousReadMode = message?.MessageRead ?? true;

            //update status for message, mark messgage to read
            var isSuccessSetRead = true;
            if (!previousReadMode) // if read mode = false (unread = true), set read mode = true
                isSuccessSetRead = await _messageRepository.SetMessageReadmode(SiteUser.UserId, new[] { msgId }, true);

            if (result.Keys.Count <= 0) return Json(new { });

            var messageStatus = await _messageRepository.GetDetailMessageStatus(SiteUser.UserId, msgId);
            var status = messageStatus?.Status ?? 0;

            //var sortedResult = result.Keys.OrderBy(x=>x.del)

            var messages = result.Select(msg => new
            {
                id = msg.Key.MessageId,
                header = msg.Key.HeadLine,
                content = msg.Key.MailMessage,
                categoryId = msg.Key.MessageArea,
                categoryName = msg.Key.AreaDescription,
                isUnRead = !isSuccessSetRead, //
                isStarred = status == (int)MessageStatus.Starred,
                isTrash = status == (int)MessageStatus.InTrash,
                isDelete = status == (int)MessageStatus.Deleted,
                receivedDate = $"{msg.Key.SendDate:yyyy-MM-dd}",
                customerAddress = msg.Key.CustomerAddress,
                customerName = msg.Key.CustomerName,
                customerZipAndCity = msg.Key.CustomerZipAndCity,
                messageTable = IsShowTableOnMessage(msg.Key.MessageType)
                    ? GetDeliveryTableInfor(msg.Key.MessageType,
                        msg.Value.Where(item => !string.IsNullOrEmpty(item.OrderNo)).OrderByDescending(x => x.PlannedDeliveryDate))
                    : null,
                messageType = message?.MessageType ?? 0,
                //deliveriesView= this.RenderPartialViewToString("~/Views/DeliveryMessagesPage/DeliveriesTables.cshtml", null)
                deliveriesView = GetPlannedDeliveriesView(msg.Key.MessageId, msg.Key.CustomerName)

            });

            return Json(new
            {
                message = messages.FirstOrDefault(),
                isNeedUpdate = previousReadMode == false && isSuccessSetRead
            });
        }

        private string GetPlannedDeliveriesView(int messageId, string customerName)
        {
            var plannedDeliveriesList = _messageRepository.GetPlannedDeliveriesList(0, messageId);
            plannedDeliveriesList = plannedDeliveriesList ?? new PlannedDeliveries[0];
            var viewMoel = new PlannedDeliveriesMessageModel()
            {
                CustomerName = customerName,
                PlannedDeliveriesList = plannedDeliveriesList
            };
            return this.RenderPartialViewToString("~/Views/DeliveryMessagesPage/DeliveriesTables.cshtml",viewMoel );
        }

        private static bool IsShowTableOnMessage(int messageType)
        {
            return messageType == 6 || messageType == 7 ||
                   messageType == 8 || messageType == 9 ||
                   messageType == 10 || messageType == 11 ||
                   messageType == 12;
            //|| messageType == 15;
        }

        private static object GetDeliveryTableInfor(int messageType, IEnumerable<MessageExtended> msgList)
        {
            var firstTableHeaders = messageType == 15 ? GetFirstTblDeliveryHeaders(messageType) : null;
            var secondTableHeaders = GetSecondTblDeliveryHeaders(messageType);

            var listForSecondTbl = msgList as IList<MessageExtended> ?? msgList.ToList();
            var firstExtendedMsg = listForSecondTbl.IsNullOrEmpty() ? null : listForSecondTbl.FirstOrDefault() ?? new MessageExtended();
            if (firstExtendedMsg == null) return null;

            var msgListForSecondTbl = messageType == 15 ? listForSecondTbl.Where(x => x.PickUp).Select(x => x) : listForSecondTbl;
            return new
            {
                carrier = firstExtendedMsg.Carrier,
                carNo = firstExtendedMsg.CarNo,
                mobileNo = firstExtendedMsg.CarMobileNo,
                freightNo = firstExtendedMsg.FreightNo,

                firstTblHeaders = firstTableHeaders,
                firstTblRows = messageType == 15 ? GetFistTblDeliveryRows(listForSecondTbl.Where(x => !x.PickUp), messageType, firstTableHeaders) : null,
                secondHeaderItems = secondTableHeaders,
                secondTableRows = msgListForSecondTbl.Select(msg => new
                {
                    items = GetDeliveryItems(msg, secondTableHeaders)
                })
            };
        }

        private static IEnumerable<object> GetFistTblDeliveryRows(IEnumerable<MessageExtended> msgList, int messageType, List<string> firstTableHeaders)
        {
            if (messageType == 15)
            {
                return msgList.Select(msg => new
                {
                    items = GetDeliveryItems(msg, firstTableHeaders)
                });
            }
            return null;
        }

        private static List<string> GetFirstTblDeliveryHeaders(int messageType)
        {
            if (messageType == 15)
            {
                return new List<string>()
                {
                    Td.Sandning,
                    Td.SummaKvantitet,
                    Td.AntalOrderrader,
                    Td.Planeradankomst,
                    Td.FranFabrikLager,
                    Td.Transportor,
                    Td.TelTransportor
                };
            }
            return null;
        }

        private static List<object> GetDeliveryItems(MessageExtended msg, List<string> headerItems)
        {
            if (headerItems.IsNullOrEmpty() || msg == null)
            {
                return new List<object>();
            }
            return headerItems.Select(header => DeliveryMessageHelper.MappingMsgInforByName(msg, header)).ToList();
        }

        private static object MappingMsgInforByName(MessageExtended msg, string header)
        {
            if (header.IsMemberOfList(Td.Ordernr, Td.Ordernumbmer)) return msg.OrderNo;
            if (header.IsMemberOfList(Td.Rad, Td.AntalOrderrader)) return msg.OrderLine;
            if (header.Equals(Td.Artikel)) return msg.ItemName;
            if (header.IsMemberOfList(Td.BestKvantitet, Td.PlaneradKvantitet, Td.BestKvant, Td.SummaKvantitet, Td.Summa)) return msg.OrderQuantity;
            if (header.Equals(Td.Silo)) return msg.Container;
            if (header.IsMemberOfList(Td.Planeradankomst, Td.PlaneradHamtning, Td.PlanAnkomst))
                return msg.PlannedDeliveryDate != null ? $"{msg.PlannedDeliveryDate:yyyy-MM-dd}" : string.Empty;
            if (header.IsMemberOfList(Td.FranFabrik, Td.FranLager, Td.LevererasTill, Td.FranFabrikLager, Td.TillLager)) return msg.Warehouse;
            if (header.IsMemberOfList(Td.Levererat, Td.Hamtat)) return msg.DeliveryDate != null ? $"{msg.DeliveryDate:yyyy-MM-dd}" : string.Empty;
            if (header.IsMemberOfList(Td.LevKvant, Td.LevKvantitet)) return msg.DeliveredQuantity;
            if (header.Equals(Td.Enhet)) return msg.Unit;
            if (header.IsMemberOfList(Td.Sandning, Td.Sandnr)) return msg.FreightNo;
            if (header.Equals(Td.Transportor)) return msg.Carrier;
            if (header.Equals(Td.Bil)) return msg.CarNo;
            if (header.Equals(Td.TelTransportor)) return msg.CarMobileNo;
            return header.IsMemberOfList(Td.TelefonBil) ? msg.CarMobileNo : null;
        }

        private static List<string> GetSecondTblDeliveryHeaders(int messageType)
        {
            switch (messageType)
            {
                case 6:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.BestKvantitet,
                        Td.Silo,
                        Td.Planeradankomst,
                        Td.FranFabrik
                    };
                case 7:
                case 8:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.BestKvantitet,
                        Td.LevKvantitet,
                        Td.Silo,
                        Td.Planeradankomst,
                        Td.Levererat,
                        Td.FranFabrik
                    };
                case 9:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.PlaneradKvantitet,
                        Td.Enhet,
                        Td.PlaneradHamtning,
                        Td.LevererasTill
                    };
                case 10:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.PlaneradKvantitet,
                        Td.Enhet,
                        Td.PlaneradHamtning,
                        Td.Hamtat,
                        Td.LevererasTill
                    };
                case 11:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.BestKvantitet,
                        Td.Enhet,
                        Td.Planeradankomst,
                        Td.FranLager
                    };
                case 12:
                    return new List<string>()
                    {
                        Td.Ordernr,
                        Td.Rad,
                        Td.Artikel,
                        Td.BestKvant,
                        Td.LevKvant,
                        Td.Enhet,
                        Td.Planeradankomst,
                        Td.Levererat,
                        Td.FranLager
                    };
                default:
                    return new List<string>();
            }
        }

        //POST: /api/user/markToUnRead
        [Route("mark-to-unread")]
        [HttpPost]
        public async Task<JsonResult> MarkToUnRead(string msgIds, bool unRead)
        {
            var messages = msgIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var messageIds = new List<int>();
            foreach (var msg in messages)
            {
                int outId;
                if (int.TryParse(msg, out outId))
                {
                    messageIds.Add(outId);
                }
            }

            var result = await _messageRepository.SetMessageReadmode(SiteUser.UserId, messageIds.ToArray(), !unRead);
            return Json(new { success = result });
        }

        //POST: /api/user/markToUnRead
        [Route("mark-to-starred")]
        [HttpPost]
        public async Task<JsonResult> MarkToStarred(int msgId, bool isStarred)
        {
            bool success;
            if (isStarred)
            {
                success = await _messageRepository.MarkAsStarred(SiteUser.UserId, new[] { msgId });
            }
            else
            {
                var updatedMessage = await _messageRepository.UpdateMessageStatus(SiteUser.UserId, msgId, (int)MessageStatus.Default);
                success = updatedMessage.Status == (int)MessageStatus.Default;
            }

            var newCategories = await GetCategoriesByMessageStatus(SiteUser.UserId, "starred");
            var jsCategories = newCategories?.Select(cat => new
            {
                id = cat.Categoryid,
                name = cat.CategoryName,
                isSelected = false
            });

            return Json(new { success = success, categories = jsCategories });
        }

        [Route("move-to-trash")]
        [HttpPost]
        public async Task<JsonResult> MoveToTrash(string msgIds)
        {
            var messages = msgIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var messageIds = new List<int>();
            foreach (var msg in messages)
            {
                int outId;
                if (int.TryParse(msg, out outId))
                {
                    messageIds.Add(outId);
                }
            }
            var result = await _messageRepository.MoveToTrash(SiteUser.UserId, messageIds.ToArray());

            var newCategories = await GetCategoriesByMessageStatus(SiteUser.UserId, "starred");
            var jsCategories = newCategories?.Select(cat => new
            {
                id = cat.Categoryid,
                name = cat.CategoryName,
                isSelected = false
            });

            return Json(new { success = result, categories = jsCategories });
        }

        [Route("delete-from-trash")]
        [HttpPost]
        public async Task<JsonResult> DeleteMessagesFromTrash(string type, string msgIds)
        {
            var messages = msgIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var messageIds = new List<int>();
            foreach (var msg in messages)
            {
                int outId;
                if (int.TryParse(msg, out outId))
                {
                    messageIds.Add(outId);
                }
            }
            var result = await _messageRepository.DeleteFromTrash(SiteUser.UserId, messageIds.ToArray());
            var newCategories = await GetCategoriesByMessageStatus(SiteUser.UserId, type);
            var jsCategories = newCategories?.Select(cat => new
            {
                id = cat.Categoryid,
                name = cat.CategoryName,
                isSelected = false
            });

            return Json(new { success = result, categories = jsCategories });
        }

        [Route("get-total")]
        [HttpPost]
        public async Task<JsonResult> OnlyGetTotalMessagesInfor()
        {
            var result = await _messageRepository.GetInboxInformation(SiteUser.UserId);
            if (result != null && result.Length >= 2)
            {
                return Json(new { inboxTotal = result[0], starredTotal = result[1] });
            }

            return Json(new { inboxTotal = 0, starredTotal = 0, });
        }

        [Route("move-to-inbox")]
        [HttpPost]
        public async Task<JsonResult> MoveToInbox(string type, string msgIds)
        {
            var messages = msgIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var result = await _messageRepository.UpdateMultipleMessagesStatus(SiteUser.UserId, messages, (int)MessageStatus.Default);

            var newCategories = await GetCategoriesByMessageStatus(SiteUser.UserId, type);
            var jsCategories = newCategories?.Select(cat => new
            {
                id = cat.Categoryid,
                name = cat.CategoryName,
                isSelected = false
            });

            return Json(new { success = result, categories = jsCategories });
        }

        [Route("get-inbox-statistics")]
        [HttpPost]
        [SkipRole]
        public async Task<JsonResult> GetInboxStatistics()
        {
            int numberUnRead = 0, numberStarred = 0;
            var userId = SiteUser?.UserId ?? 0;
            if (userId <= 0) return Json(new { totalUnRead = numberUnRead, totalStarred = numberStarred });

            var totalUreadMess = _messageRepository.GetTotalUnreadMessages(userId);
            var inboxInfo = await _messageRepository.GetInboxInformation(userId);

            if (inboxInfo != null && inboxInfo.Length >= 2)
            {
                numberStarred = inboxInfo[1];
            }
            if (totalUreadMess > 0)
            {
                numberUnRead = totalUreadMess;
            }

            return Json(new { totalUnRead = numberUnRead, totalStarred = numberStarred });
        }


        private async Task<Category[]> GetCategoriesByMessageStatus(int userId, string type)
        {
            Category[] newCategories = null;
            if (!string.IsNullOrEmpty(type) && type.Equals("starred", StringComparison.OrdinalIgnoreCase))
            {
                var starred = new[] { ((int)MessageStatus.Starred).ToString() };
                newCategories = await _messageRepository.GetCategoriesByStatus(userId, starred);
            }
            else if (!string.IsNullOrEmpty(type) && type.Equals("trash", StringComparison.OrdinalIgnoreCase))
            {
                var trash = new[] { ((int)MessageStatus.InTrash).ToString() };
                newCategories = await _messageRepository.GetCategoriesByStatus(userId, trash);
            }

            return newCategories;
        }
    }
}
