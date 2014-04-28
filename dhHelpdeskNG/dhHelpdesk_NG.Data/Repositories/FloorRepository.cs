namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IFloorRepository : IRepository<Floor>
    {
        List<ItemOverview> FindOverviews(int customerId);
    }

    public class FloorRepository : RepositoryBase<Floor>, IFloorRepository
    {
        public FloorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public List<ItemOverview> FindOverviews(int customerId)
        {
            var anonymus =
                this.DataContext.Floors
                    .Where(x => x.Building.Customer_Id == customerId)
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}
