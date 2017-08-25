using System.Collections.Generic;
using Gro.Core.DataModels.DeliveryNoteDtos;

namespace Gro.Core.Interfaces
{
    public interface IDeliveryNoteRepository
    {
        IEnumerable<FoljesedelResponse> GetFoljesedlar(string customerNumber, string ticket);

        string GetPDF(string orderNumber, int fabricId, int rowNumber, string ticket);
    }
}
