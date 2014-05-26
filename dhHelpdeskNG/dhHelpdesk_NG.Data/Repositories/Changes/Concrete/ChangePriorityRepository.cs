namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangePriorityRepository : RepositoryBase<ChangePriorityEntity>, IChangePriorityRepository
    {
        public ChangePriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var priorities =
                this.DataContext.ChangePriorities.Where(p => p.Customer_Id == customerId)
                    .Select(p => new { p.Id, p.ChangePriority })
                    .ToList();

            return
                priorities.Select(p => new ItemOverview(p.ChangePriority, p.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public string GetPriorityName(int priorityId)
        {
            return this.DataContext.Priorities.Where(p => p.Id == priorityId).Select(p => p.Name).FirstOrDefault();
        }
    }
}