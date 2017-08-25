using Gro.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Gro.Core.DataModels.Grain;
using Gro.Infrastructure.Data.GrainService;

namespace Gro.Infrastructure.Data.Repositories
{
    public class GrainRepository : IGrainRepository
    {
        private readonly IGrainService _service;

        public GrainRepository(IGrainService service)
        {
            _service = service;
        }

        public List<AgreementsDeliverys> GetAgreementsDeliverysThreeLatest(string supplierNumber, string ticket)
            => _service.GetAgreementsDeliverysThreeLatest(supplierNumber, ticket).ToList();

        public List<Deliverys> GetDeliverysFiveLatest(string supplierNumber, string ticket)
            => _service.GetDeliverysFiveLatest(supplierNumber, ticket).ToList();
    }
}
