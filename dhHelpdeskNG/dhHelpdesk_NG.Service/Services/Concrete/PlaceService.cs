namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Repositories;

    public interface IPlaceService
    {
        List<ItemOverview> GetBuildings(int customerId);

        List<ItemOverview> GetFloors(int customerId);

        List<ItemOverview> GetFloors(int customerId, int? buildingId);

        List<ItemOverview> GetRooms(int customerId);

        List<ItemOverview> GetRooms(int customerId, int? floorId);
    }

    public class PlaceService : IPlaceService
    {
        private readonly IBuildingRepository buildingRepository;

        private readonly IFloorRepository floorRepository;

        private readonly IRoomRepository roomRepository;

        public PlaceService(
            IBuildingRepository buildingRepository,
            IFloorRepository floorRepository,
            IRoomRepository roomRepository)
        {
            this.buildingRepository = buildingRepository;
            this.floorRepository = floorRepository;
            this.roomRepository = roomRepository;
        }

        public List<ItemOverview> GetBuildings(int customerId)
        {
            return this.buildingRepository.FindOverviews(customerId);
        }

        public List<ItemOverview> GetFloors(int customerId)
        {
            return this.floorRepository.FindOverviews(customerId);
        }

        public List<ItemOverview> GetFloors(int customerId, int? buildingId)
        {
            return !buildingId.HasValue
                       ? this.GetFloors(customerId)
                       : this.floorRepository.FindOverviews(customerId, buildingId.Value);
        }

        public List<ItemOverview> GetRooms(int customerId)
        {
            return this.roomRepository.FindOverviews(customerId);
        }

        public List<ItemOverview> GetRooms(int customerId, int? floorId)
        {
            return !floorId.HasValue
                ? this.GetRooms(customerId)
                : this.roomRepository.FindOverviews(customerId, floorId.Value);
        }
    }
}