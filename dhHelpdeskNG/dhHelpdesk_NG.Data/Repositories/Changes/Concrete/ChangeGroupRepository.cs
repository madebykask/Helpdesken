namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeGroupRepository : RepositoryBase<ChangeGroupEntity>, IChangeGroupRepository
    {
        public ChangeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var groups =
                this.DataContext.ChangeGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { g.Id, g.ChangeGroup })
                    .ToList();

            return
                groups.Select(g => new ItemOverview(g.ChangeGroup, g.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }
    }
}