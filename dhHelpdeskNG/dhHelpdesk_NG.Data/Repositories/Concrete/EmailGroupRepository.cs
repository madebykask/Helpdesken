namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class EmailGroupRepository : RepositoryBase<EmailGroupEntity>, IEmailGroupRepository
    {
        public EmailGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            var emailGroups =
                this.DataContext.EMailGroups.Where(g => g.Customer_Id == customerId)
                    .Select(g => new { g.Id, Name = g.Name })
                    .ToList();

            return
                emailGroups.Select(g => new ItemOverview(g.Name, g.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}
