using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Core.DataModels.MessageHubDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.MessageHubService;
using System.Linq;

namespace Gro.Infrastructure.Data.Repositories
{
    public class UserMessageSettingsRepository : IUserMessageSettingsRepository
    {
        private readonly IMessagehubService _messageHubService;

        public UserMessageSettingsRepository(IMessagehubService messageHubService)
        {
            _messageHubService = messageHubService;
        }

        /// <summary>
        /// Get the message settings display items
        /// </summary>
        public async Task<IEnumerable<MessageSettingTab>> GetSettingDisplayAsync(int customerId, int organizationId)
        {
            var result = await _messageHubService.GetMsgSettingsDisplayAsync(customerId, organizationId);
            foreach (var tab in result.SettingsTab)
            {
                //merge all categories with the same id
                var mergedCategories = tab.Category
                    .GroupBy(c => c.Categoryid)
                    .Select(group =>
                    {
                        var aggregatedSettings = group.Aggregate(new List<MsgType>(), (l, c) =>
                        {
                            l.AddRange(c.MessageType);
                            return l;
                        });
                        return new Category
                        {
                            Categoryid = group.Key,
                            CategoryName = group.First().CategoryName,
                            MessageType = aggregatedSettings.ToArray()
                        };
                    })
                    .OrderBy(x => x.Categoryid)
                    .ToArray();

                tab.Category = mergedCategories;
            }

            return result.SettingsTab;
        }

        public async Task<bool> SaveSettings(MessageSettings[] settings)
        {
            var tasks = await Task.WhenAll(settings.Select(s => _messageHubService.SaveMessageSettingsAsync(new[] { s })));
            var result = tasks.Aggregate(true, (s, v) => s && v);

            return result;
        }
    }
}
