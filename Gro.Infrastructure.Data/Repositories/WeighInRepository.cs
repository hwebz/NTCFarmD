using Gro.Core.Interfaces;
using Gro.Core.DataModels.WeighInDtos;
using Gro.Infrastructure.Data.WeighInService;
using System.Threading.Tasks;
using Gro.Infrastructure.Data.Interceptors.Attributes;

namespace Gro.Infrastructure.Data.Repositories
{
    public class WeighInRepository : IWeighInRepository
    {
        private readonly IWeighInService _service;

        public WeighInRepository(IWeighInService service)
        {
            _service = service;
        }

        [Cache]
        public async Task<AnalyzeList[]> GetAnalyzeListAsync(string supplier, int deliveryNumber, string ticket)
            => await _service.GetAnalyzeListAsync(supplier, deliveryNumber, ticket) ?? new AnalyzeList[0];

        [Cache]
        public async Task<WeighInExtended[]> GetMoreInfoAsync(string supplier, int deliveryNumber, string ticket)
            => await _service.GetMoreInfoAsync(supplier, deliveryNumber, ticket) ?? new WeighInExtended[0];

        [Cache]
        public async Task<Overview[]> GetOverViewListAsync(string supplier, int year, string ticket)
            => await _service.GetOverViewListAsync(supplier, year, ticket) ?? new Overview[0];

        [Cache]
        public Task<WeighIn> GetWeighInAsync(string supplier, string agreementNumber, string ticket)
            => _service.GetWeighInAsync(supplier, agreementNumber, ticket);

        [Cache]
        public async Task<WeighIn[]> GetWeighInListAsync(string supplier, int year, string ticket)
            => await _service.GetWeighInListAsync(supplier, year, ticket) ?? new WeighIn[0];

        [Cache]
        public async Task<WeighInSumAgreementDto[]> GetWeighInSumAgreementAsync(string skordear, string leverantorsnummer, string ticket)
            => await _service.GetWeighInSumAgreementAsync(skordear, leverantorsnummer, ticket) ?? new WeighInSumAgreementDto[0];

    }
}
