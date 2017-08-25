using System.Collections.Generic;
using Gro.Core.DataModels.MessageHubDtos;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IMessageAdministrationRepository
    {
        Task<Category[]> GetAllCategories(bool showTipsAndInfo);
        Task<Dictionary<Message, MessageExtended[]>> GetMessage(int messageId, int userId);
        Task<IEnumerable<Message>> GetMessages(int pageSize, int pageIndex, string[] categories, string[] messageTypes);
        Task<int> CountMessages(string[] listCategories, string[] listMessageTypes);
        Task<string[]> GetReceivers(int messageId);

        Task<int> SaveAdhocMessage(int areaId, string emailReceivers, string smsReceivers, string smsSender, string headLine, string mailMessage, string smsMessage);

        Task<int> SaveStandardMessage(int areaId, int typeId, string emailReceivers, string smsReceivers, string smsSender, string headLine, string mailMessage, string smsMessage);

        Task<Category[]> GetFreeMessageCategories();
    }
}
