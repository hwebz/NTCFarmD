using Gro.Core.DataModels.Grain;
using System.Collections.Generic;

namespace Gro.Core.Interfaces
{
    public interface IGrainRepository
    {
        List<AgreementsDeliverys> GetAgreementsDeliverysThreeLatest(string supplierNumber, string ticket);
        List<Deliverys> GetDeliverysFiveLatest(string supplierNumber, string ticket);
    }
}
