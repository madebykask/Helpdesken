namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class ChangeGroupRepository : RepositoryBase<ChangeGroupEntity>, IChangeGroupRepository
    {
        public ChangeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindOverviews(int customerId)
        {
            var groups =
                this.DataContext.ChangeGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { Name = g.ChangeGroup, g.Id })
                    .ToList();

            return
                groups.Select(
                    g => new ItemOverviewDto { Name = g.Name, Value = g.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }
    }
}