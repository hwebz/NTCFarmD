using Gro.Core.DataModels.Boka;
using Gro.Core.DataModels.Boka.DeleteReservation;
using Gro.Core.DataModels.Boka.UpdateReservation;
using System;
using System.Collections.Generic;
using Gro.Core.DataModels.Boka.ListingBoka;

namespace Gro.Core.Interfaces
{
    public interface IBokaRepository
    {
        List<ResourceItemDto> GetResourceGroupList(bool allResource);
        List<ResourceItemDto> GetResourceGroupsOnIo(string warehouseId);
        ResourceGroupItemsDto LoadItemsOnresourceGroup(string resourceGroupId, string selectedDate, string currentArticleItem, bool showOnlyUnloadingItems);
        SearchResultDto CustomerSearch(string resourceGroupId, string searchString, string searchType, string loadOrUnlodValue, string customerSearcType, string customerNo, string username);
        List<ResourceGroupItemDto> SearchAvailbleSlots(string resourceGroupId, string selectedDate, string article, string qty, string loadunload, string veichleType, string driedUnDried, string customerNo, string searchType);
        bool ExistBooking(string resourceGroupId, string selectedDate, string article, string qty, string loadunload, string veichleType, string driedUnDried, string customerNo, string searchType, string iONumber);
        List<ReservationResultDto> MakeReservation(MakeReservationDto reservationToMake, string customerNo);
        List<DeleReservationResultDto> DeleteReservation(string reservationId, string owner, string customerNo, DateTime dateRegistered);
        List<ReservationResultDto> UpdateReservation(UpdateReservationDto reservationToUpdate, string customerNo);
        List<SearchBookingsDto> SearchBookings(RequestSearchBookings requestSearch);
        List<CustomerDto> GetCustomers(string customerNo, string customerSearchType, bool isInternal, string currentUser);
    }
}
