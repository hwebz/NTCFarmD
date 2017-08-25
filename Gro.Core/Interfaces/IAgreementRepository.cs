using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.DataModels.AgreementDtos;

namespace Gro.Core.Interfaces
{
    public interface IAgreementRepository
    {
        IEnumerable<IGrouping<int, Agreement>> GetAgreementsListByYears(string supplier);
        List<Agreement> GetAgreementsList(string supplier);
        Task<Agreement[]> GetAgreementsListAsync(string supplier);

        IEnumerable<IGrouping<int, SeedAssurance>> GetSeedAgreementsByYears(string supplier);
        List<SeedAssurance> GetSeedAgreements(string supplier);

        IEnumerable<IGrouping<int, Agreement>> GetFarmingAgreementsByYears(string supplier);
        List<Agreement> GetFarmingAgreements(string supplier);
        List<DryAgreement> GetDryAgreements(string supplier);
        List<PriceHedging> GetPriceHedgingList(string supplier, string agreementNumber);
        Task<PriceHedging[]> GetPriceHedgingListAsync(string supplier, string agreementNumber);
        Task<FarmSample[]> GetFarmSamplesListAsync(string supplier, string agreementNumber);

        Agreement[] GetOpenGrainAgreements(string customerNumber, int count);
        SeedAssurance[] GetOpenSeedAgreements(string customerNumber, int count);
    }
}
