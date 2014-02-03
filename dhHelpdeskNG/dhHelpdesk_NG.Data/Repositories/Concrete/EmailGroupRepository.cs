namespace dhHelpdesk_NG.Data.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class EmailGroupRepository : RepositoryBase<EmailGroupEntity>, IEmailGroupRepository
    {
        public EmailGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverviewDto> FindActiveOverviews(int customerId)
        {
            var emailGroups =
                this.DataContext.EMailGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { g.Id, Name = g.Name })
                    .ToList();

            return
                emailGroups.Select(
                    g => new ItemOverviewDto { Name = g.Name, Value = g.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }
    }
}
