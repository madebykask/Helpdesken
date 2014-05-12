namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IRoomRepository : IRepository<Room>
    {
        List<ItemOverview> FindOverviews(int customerId);

        List<ItemOverview> FindOverviews(int customerId, int floorId);
    }

    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var anonymus =
                this.DataContext.Rooms
                    .Where(x => x.Floor.Building.Customer_Id == customerId)
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }

        public List<ItemOverview> FindOverviews(int customerId, int floorId)
        {
            var anonymus =
                this.DataContext.Rooms
                    .Where(x => x.Floor.Building.Customer_Id == customerId && x.Floor_Id == floorId)
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}
