using System.Collections.Generic;
using Gro.Core.DataModels.DeliveryNoteDtos;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.DeliveryNoteService;
using Gro.Infrastructure.Data.Interceptors.Attributes;

namespace Gro.Infrastructure.Data.Repositories
{
    public class DeliveryNoteRepository : IDeliveryNoteRepository
    {
        private readonly IFoljesedelService _service;

        public DeliveryNoteRepository(IFoljesedelService service)
        {
            _service = service;
        }

        [Cache]
        public IEnumerable<FoljesedelResponse> GetFoljesedlar(string customerNumber, string ticket)
            => _service.Search(null, customerNumber, null, null, ticket);

        public string GetPDF(string orderNumber, int fabricId, int rowNumber, string ticket)
            => _service.GetPDFFile(orderNumber, fabricId, rowNumber, ticket);
    }
}
