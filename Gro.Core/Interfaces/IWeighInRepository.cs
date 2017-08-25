using Gro.Core.DataModels.WeighInDtos;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IWeighInRepository
    {
        Task<AnalyzeList[]> GetAnalyzeListAsync(string supplier, int deliveryNumber, string ticket);

        Task<WeighIn> GetWeighInAsync(string supplier, string agreementNumber, string ticket);

        Task<WeighInExtended[]> GetMoreInfoAsync(string supplier, int deliveryNumber, string ticket);

        Task<Overview[]> GetOverViewListAsync(string supplier, int year, string ticket);

        Task<WeighIn[]> GetWeighInListAsync(string supplier, int year, string ticket);

        Task<WeighInSumAgreementDto[]> GetWeighInSumAgreementAsync(string skordear, string leverantorsnummer, string ticket);
    }
}
