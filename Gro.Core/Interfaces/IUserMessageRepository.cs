using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.DataModels.MyProfile;

namespace Gro.Core.Interfaces
{
    public interface IUserMessageRepository
    {
        Task<EnvTypes[]> GetAllCategories(int userId);

        Task<Category[]> GetCategoriesByStatus(int userId, string[] statusIds);

        Task<Message[]> GetMessagesForUser(int userId, int pageSize, int pageIndex, string allOrUnRead, string[] categories, string[] statusIds);

        Task<Dictionary<Message, MessageExtended[]>> GetDetailMessage(int msgId, int userId);

        Task<UserMessage> GetDetailMessageStatus(int userId, int msgId);

        Task<int> CountMessages(int userId, string allOrUnRead, string[] categories, string[] statusIds);

        Task<bool> MoveToTrash(int userId, int[] msgId);

        Task<bool> MarkAsStarred(int userId, int[] msgId);

        Task<Message> UpdateMessageStatus(int userId, int msgId, int statusId);

        Task<bool> UpdateMultipleMessagesStatus(int userId, string[] msgIds, int statusId);

        Task<bool> SetMessageReadmode(int userId, int[] msgId, bool read);

        Task<bool> DeleteFromTrash(int userId, int[] msgIds);

        Task<bool> EmptyTrash(int userId);

        Task<int[]> GetInboxInformation(int userId);

        Task<int> GetTotalUnreadMessagesAsync(int userId);

        int GetTotalUnreadMessages(int userId);
        Task<EnvTypes[]> GetCategoriesForFiltering(int userId);

        DeliveryInformation GetDeliverySummary(int customerId, int maximumOfDeliveyItems);

        PlannedDeliveries GetPlannedDeliveries(int customerId);
        PlannedDeliveries[] GetPlannedDeliveriesList(int customerId, int messId);
    }
}
