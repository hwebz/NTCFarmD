using Gro.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gro.Core.DataModels.MessageHubDtos;
using System.Collections.Generic;
using Gro.ViewModels.Messaging;

namespace Gro.Controllers.Apis
{
    [RoutePrefix("api/message-admin")]
    [MessageAdminRoles]
    public class MessageAdminController : Controller
    {
        private readonly IMessageAdministrationRepository _messageAdminRepo;
        public MessageAdminController(IMessageAdministrationRepository messageAdminRepo)
        {
            _messageAdminRepo = messageAdminRepo;
        }

        [Route("all-categories")]
        public async Task<JsonResult> GetAllCategoriesAsync(bool showTipsAndInfo)
        {
            var queryResult = await _messageAdminRepo.GetAllCategories(showTipsAndInfo);
            return Json(queryResult ?? new Category[0], JsonRequestBehavior.AllowGet);
        }

        [Route("free-categories")]
        public async Task<JsonResult> GetFreeCategoriesAsync()
        {
            var queryResult = await _messageAdminRepo.GetFreeMessageCategories();
            return Json(queryResult ?? new Category[0], JsonRequestBehavior.AllowGet);
        }

        //GET /api/message-admin/message-detail/{messageId}
        [Route("message-detail/{messageId}")]
        public async Task<JsonResult> GetMessageDetail(int messageId)
        {
            var result = await _messageAdminRepo.GetMessage(messageId, 0);
            if (result == null || result.Count == 0)
            {
                Response.StatusCode = 404;
                return Json("MessageNotFound", JsonRequestBehavior.AllowGet);
            }

            var receivers = await _messageAdminRepo.GetReceivers(messageId);
            receivers = receivers ?? new string[0];

            var message = result.ElementAt(0);
            return Json(new
            {
                Message = message.Key,
                ExtendedInfo = message.Value,
                Receivers = receivers
            }, JsonRequestBehavior.AllowGet);
        }

        //GET /api/message-admin/message-detail/{messageId}
        [Route("get-messages")]
        [HttpPost]
        public async Task<JsonResult> GetListMessagesAsync(int pageSize, int pageIndex, string[] categories, string[] messageTypes)
        {
            var listCategories = categories ?? new string[0];
            var listMessageTypes = messageTypes ?? new string[0];

            var listMessage = await _messageAdminRepo.GetMessages(pageSize, pageIndex, listCategories, listMessageTypes);
            listMessage = listMessage ?? new List<Message>();
            return Json(listMessage);
        }

        [Route("get-totalmessage")]
        [HttpPost]
        public async Task<JsonResult> GetTotalMessageAsync(string[] categories, string[] messageTypes)
        {
            var listCategories = categories ?? new string[0];
            var listMessageTypes = messageTypes ?? new string[0];

            var totalMess = await _messageAdminRepo.CountMessages(listCategories, listMessageTypes);
            return Json(totalMess);
        }

        //POST /api/message-admin/save-adhoc-message
        [Route("save-adhoc-message")]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveAdhocMessageAsync(SaveAdhocMessageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var error = ModelState.FirstOrDefault().Value.Errors.FirstOrDefault()?.ErrorMessage;
                return Json(error);
            }

            var newId = await _messageAdminRepo.SaveAdhocMessage(viewModel.AreaId, viewModel.EmailReceivers,
                viewModel.SmsReceivers, viewModel.SmsSender,
                viewModel.HeadLine, viewModel.MailMessage, viewModel.SmsMessage);

            if (newId > 0) return Json(new { newId = newId });

            Response.StatusCode = 500;
            return Json("ServerError");
        }

        //POST /api/message-admin/save-standard-message
        [Route("save-standard-message")]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SaveStandardMessageAsync(SaveStandardMessageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json("BadRequest");
            }

            var newId = await _messageAdminRepo.SaveStandardMessage(viewModel.AreaId, viewModel.TypeId, viewModel.EmailReceivers,
                viewModel.SmsReceivers, viewModel.SmsSender,
                viewModel.HeadLine, viewModel.MailMessage, viewModel.SmsMessage);

            if (newId > 0) return Json(new { newId = newId });
            Response.StatusCode = 500;
            return Json("ServerError");
        }
    }
}
