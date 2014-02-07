namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public class ChangePriorityRepository : RepositoryBase<ChangePriorityEntity>, IChangePriorityRepository
    {
        public ChangePriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var priorities =
                this.DataContext.ChangePriorities.Where(p => p.Customer_Id == customerId)
                    .Select(p => new { p.Name, p.Id })
                    .ToList();

            return
                priorities.Select(p => new ItemOverview(p.Name, p.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}