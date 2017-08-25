using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.DataModels.AgreementDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.AgreementService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class AgreementRepository : IAgreementRepository
    {
        private readonly IAgreementService _service;
        private readonly TicketProvider _ticketProvider;

        private string Ticket => _ticketProvider?.GetTicket();

        public AgreementRepository(IAgreementService service, TicketProvider ticketProvider)
        {
            _service = service;
            _ticketProvider = ticketProvider;
        }

        //[Cache]
        public IEnumerable<IGrouping<int, Agreement>> GetAgreementsListByYears(string supplier)
        {
            var agreements = GetAgreementsList(supplier);
            agreements = agreements ?? new List<Agreement>();
            return agreements.Any() ? agreements.GroupBy(x => x.HarvestYear) : new List<IGrouping<int, Agreement>>();
        }

        //[Cache]
        public List<Agreement> GetAgreementsList(string supplier)
            => _service.GetAgreementList(supplier, Ticket)
                .OrderByDescending(x => x.HarvestYear)
                .ThenBy(x => x.ValidFrom)
                .ThenBy(x => x.ValidTo)
                .ToList();

        //[Cache]
        public Task<Agreement[]> GetAgreementsListAsync(string supplier)
            => _service.GetAgreementListAsync(supplier, Ticket);

        //[Cache]
        public IEnumerable<IGrouping<int, SeedAssurance>> GetSeedAgreementsByYears(string supplier)
        {
            var seedAgreements = GetSeedAgreements(supplier);
            seedAgreements = seedAgreements ?? new List<SeedAssurance>();
            return seedAgreements.Any() ? seedAgreements.GroupBy(x => x.HarvestYear) : new List<IGrouping<int, SeedAssurance>>();
        }

        //[Cache]
        public IEnumerable<IGrouping<int, Agreement>> GetFarmingAgreementsByYears(string supplier)
        {
            var farmAgreements = GetFarmingAgreements(supplier);
            farmAgreements = farmAgreements ?? new List<Agreement>();

            return farmAgreements.Any() ? farmAgreements.GroupBy(x => x.HarvestYear) : new List<IGrouping<int, Agreement>>();
        }

        //[Cache]
        public List<Agreement> GetFarmingAgreements(string supplier)
            => _service.GetFarmingAgreementList(supplier, Ticket).OrderByDescending(x => x.HarvestYear).ToList();

        //[Cache]
        public List<DryAgreement> GetDryAgreements(string supplier)
            => _service.GetDryAgreementList(supplier, Ticket).ToList();

        //[Cache]
        public List<PriceHedging> GetPriceHedgingList(string supplier, string agreementNumber)
            => _service.GetPriceHedgingList(supplier, agreementNumber, Ticket).ToList();

        //[Cache]
        public Task<PriceHedging[]> GetPriceHedgingListAsync(string supplier, string agreementNumber)
            => _service.GetPriceHedgingListAsync(supplier, agreementNumber, Ticket);

        //[Cache]
        public Task<FarmSample[]> GetFarmSamplesListAsync(string supplier, string agreementNumber)
            => _service.GetFarmSampleListAsync(supplier, agreementNumber, Ticket);

        //[Cache]
        public List<SeedAssurance> GetSeedAgreements(string supplier)
            => _service.GetSeedAgreementList(supplier, Ticket).OrderByDescending(x => x.HarvestYear).ToList();

        //[Cache]
        public Agreement[] GetOpenGrainAgreements(string customerNumber, int count)
            => _service.GetMyOpenAgreements(customerNumber, count, Ticket);

        //[Cache]
        public SeedAssurance[] GetOpenSeedAgreements(string customerNumber, int count)
            => _service.GetMyOpenSeedAgreements(customerNumber, count, Ticket);

    }
}
