using Gro.Core.DataModels.GrobarhetDtos;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IGrobarhetRepository
    {
        Task<GrobarhetResponse[]> GetGrobarhetAsync(string reference, string ticket);
    }
}
