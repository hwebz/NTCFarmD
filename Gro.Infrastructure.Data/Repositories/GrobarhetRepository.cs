using Gro.Core.Interfaces;
using System.Threading.Tasks;
using Gro.Core.DataModels.GrobarhetDtos;
using Gro.Infrastructure.Data.GrobarhetService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class GrobarhetRepository : IGrobarhetRepository
    {
        private readonly IGrobarhetService _service;

        public GrobarhetRepository(IGrobarhetService service)
        {
            _service = service;
        }

        public async Task<GrobarhetResponse[]> GetGrobarhetAsync(string reference, string ticket)
            => await _service.GetGrobarhetAsync(reference, ticket) ?? new GrobarhetResponse[0];
    }
}
