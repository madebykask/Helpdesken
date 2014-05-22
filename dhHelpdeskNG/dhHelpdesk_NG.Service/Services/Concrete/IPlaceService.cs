namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public interface IPlaceService
    {
        List<ItemOverview> GetBuildings(int customerId);

        List<ItemOverview> GetFloors(int customerId);

        List<ItemOverview> GetFloors(int customerId, int? buildingId);

        List<ItemOverview> GetRooms(int customerId);

        List<ItemOverview> GetRooms(int customerId, int? floorId);
    }
}