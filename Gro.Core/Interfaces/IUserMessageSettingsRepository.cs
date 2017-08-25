using Gro.Core.DataModels.MessageHubDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IUserMessageSettingsRepository
    {
        Task<IEnumerable<MessageSettingTab>> GetSettingDisplayAsync(int customerId, int organizationId);
        Task<bool> SaveSettings(MessageSettings[] settings);
    }
}
