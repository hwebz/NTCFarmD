using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.MessageHubService;
using Gro.Core.DataModels.MyProfile;
using PlannedDeliveries = Gro.Core.DataModels.MessageHubDtos.PlannedDeliveries;

namespace Gro.Infrastructure.Data.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private readonly IMessagehubService _messageHubService;

        private readonly TicketProvider _ticketProvider;

        private string Ticket => _ticketProvider.GetTicket();

        public UserMessageRepository(IMessagehubService messageHubService, TicketProvider ticketProvider)
        {
            _messageHubService = messageHubService;
            _ticketProvider = ticketProvider;
        }
     

        public async Task<EnvTypes[]> GetAllCategories(int userId)
        {
            var categories = await _messageHubService.GetCategoriesAsync(userId);
            return categories;
        }

        public async Task<Category[]> GetCategoriesByStatus(int userId, string[] statusIds)
        {
            var result = await _messageHubService.GetCategoriesByStatusAsync(userId, statusIds);
            return result?.Select(c => new Category()
            {
                Categoryid = c.EnvTypesId,
                CategoryName = c.EnvTypeDescription
            }).ToArray() ?? new Category[0];
        }

        public async Task<Message[]> GetMessagesForUser(int userId, int pageSize, int pageIndex, string allOrUnRead,
            string[] categories, string[] statusIds)
        {
            var messages = await _messageHubService.GetMessagesAsync(userId, pageSize, pageIndex, allOrUnRead, categories,
                        new string[] { }, statusIds);

            return messages;
            
        }

        public Task<Dictionary<Message, MessageExtended[]>> GetDetailMessage(int msgId, int userId) => _messageHubService.GetMessageAsync(msgId, userId);

        public Task<UserMessage> GetDetailMessageStatus(int userId, int msgId) => _messageHubService.GetUserMessageAsync(userId, msgId);

        public Task<int> CountMessages(int userId, string allOrUnRead, string[] categories, string[] statusIds)
            => _messageHubService.CountMessagesAsync(userId, allOrUnRead, categories, new string[0], statusIds);

        public Task<bool> MoveToTrash(int userId, int[] msgId) => _messageHubService.MoveToTrashAsync(userId, msgId);

        public Task<bool> MarkAsStarred(int userId, int[] msgId) => _messageHubService.MarkAsStaredAsync(userId, msgId);

        public Task<Message> UpdateMessageStatus(int userId, int msgId, int statusId)
            => _messageHubService.UpdateMessageStatusAsync(userId, msgId, statusId);

        public Task<bool> UpdateMultipleMessagesStatus(int userId, string[] msgIds, int statusId)
            => _messageHubService.UpdateMultipleMessagesStatusAsync(userId, msgIds, statusId);


        public Task<bool> SetMessageReadmode(int userId, int[] msgId, bool read)
            => _messageHubService.SetMessageReadmodeAsync(userId, msgId, read);

        public Task<bool> DeleteFromTrash(int userId, int[] msgIds)
            => _messageHubService.DeleteFromTrashAsync(userId, msgIds);

        public Task<bool> EmptyTrash(int userId) => _messageHubService.EmptyTrashAsync(userId);

        public Task<int[]> GetInboxInformation(int userId) => _messageHubService.GetInboxInformationAsync(userId);

        public async Task<int> GetTotalUnreadMessagesAsync(int userId)
        {
            var status = new[]
            {
                MessageStatus.Default.ToString(),
                MessageStatus.Archived.ToString(),
                MessageStatus.Starred.ToString()
            };
            var result = await _messageHubService.CountMessagesAsync(userId, "UnRead", new string[0], new string[0], status);
            return result;
        }

        public int GetTotalUnreadMessages(int userId)
        {
            var status = new[]
            {
                ((int) MessageStatus.Default).ToString(),
                ((int) MessageStatus.Archived).ToString(),
                ((int) MessageStatus.Starred).ToString()
            };
            return _messageHubService.CountMessages(userId, "UnRead", new string[0], new string[0], status);
        }

        public Task<EnvTypes[]> GetCategoriesForFiltering(int userId)
        {
            return _messageHubService.GetCategoriesForFilterAsync(userId);
        }

        public DeliveryInformation GetDeliverySummary(int customerId, int maximumOfDeliveyItems)
        {
            return _messageHubService.GetDeliverySummary(customerId, maximumOfDeliveyItems);
        }

        public PlannedDeliveries GetPlannedDeliveries(int customerId)
        {
            return _messageHubService.GetPlannedDeliveries(customerId, Ticket);
        }

        public PlannedDeliveries[] GetPlannedDeliveriesList(int customerId, int messId)
        {
            return _messageHubService.GetPlannedDeliveriesList(customerId, messId, Ticket);
        }
    }
}
