namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public class ChangePriorityRepository : RepositoryBase<ChangePriority>, IChangePriorityRepository
    {
        public ChangePriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviews(int customerId)
        {
            var priorities =
                this.DataContext.ChangePriorities.Where(p => p.Customer_Id == customerId)
                    .Select(p => new { p.Name, p.Id })
                    .ToList();

            return
                priorities.Select(
                    p => new ItemOverviewDto { Name = p.Name, Value = p.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }
    }
}