namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeObjectRepository : RepositoryBase<ChangeObjectEntity>, IChangeObjectRepository
    {
        public ChangeObjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var objects = this.DataContext.ChangeObjects.Where(o => o.Customer_Id == customerId);
            var overviews = objects.Select(o => new { Name = o.Name, Value = o.Id }).ToList();

            return
                overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}