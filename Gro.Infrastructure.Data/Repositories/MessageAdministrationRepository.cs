using Gro.Core.Interfaces;
using System.Threading.Tasks;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Infrastructure.Data.MessageHubService;
using System.Collections.Generic;
using System.Linq;

namespace Gro.Infrastructure.Data.Repositories
{
    public class MessageAdministrationRepository : IMessageAdministrationRepository
    {
        private readonly IMessagehubService _messageService;

        public MessageAdministrationRepository(IMessagehubService messageService)
        {
            _messageService = messageService;
        }

        public async Task<Category[]> GetAllCategories(bool showTipsAndInfo)
        {
            var categories = await _messageService.Admin_GetCategoriesAndTypesAsync(showTipsAndInfo);
            return categories;
        }

        public async Task<Dictionary<Message, MessageExtended[]>> GetMessage(int messageId,int userId)
        {
            var message = await _messageService.GetMessageAsync(messageId, userId);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessages(int pageSize, int pageIndex, string[] categories, string[] messageTypes)
        {
            var listMessages = await _messageService.Admin_GetMessagesAsync(pageSize, pageIndex, categories, messageTypes, new string[0]);
            return listMessages;
        }

        public async Task<int> CountMessages(string[] listCategories, string[] listMessageTypes)
        {
            var totalMess = await _messageService.Admin_CountMessagesAsync("all", listCategories, listMessageTypes, null);
            return totalMess;
        }

        public async Task<string[]> GetReceivers(int messageId)
        {
            var receivers = await _messageService.GetReceiversAsync(messageId);
            return receivers;
        }


        public Task<int> SaveAdhocMessage(int areaId, string emailReceivers, string smsReceivers, string smsSender, string headLine, string mailMessage, string smsMessage)
            => _messageService.SaveAdHocMessageAsync(areaId, emailReceivers, smsReceivers, smsSender, headLine, mailMessage, smsMessage);

        public Task<int> SaveStandardMessage(int areaId, int typeId, string emailReceivers, string smsReceivers, string smsSender, string headLine, string mailMessage, string smsMessage)
            => _messageService.SaveStandardMessageAsync(areaId, typeId, emailReceivers, smsReceivers, smsSender, headLine, mailMessage, smsMessage);

        public async Task<Category[]> GetFreeMessageCategories()
        {
            var results = await _messageService.GetCategoriesForAdHocAsync();
            return results.Select(r => new Category
            {
                Categoryid = r.EnvTypesId,
                CategoryName = r.EnvTypeDescription,
                MessageType = new MsgType[0]
            }).ToArray();
        }
    }
}
