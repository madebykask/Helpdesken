﻿namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class EmailGroupRepository : RepositoryBase<EmailGroupEntity>, IEmailGroupRepository
    {
        public EmailGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<IdAndNameOverview> FindActiveIdAndNameOverviews(int customerId)
        {
            var emailGroups =
                this.DataContext.EMailGroups.Where(g => g.Customer_Id == customerId && g.IsActive == 1)
                    .Select(g => new { g.Id, g.Name }).OrderBy(g => g.Name)
                    .ToList();

            return emailGroups.Select(g => new IdAndNameOverview(g.Id, g.Name)).ToList();
        }
    }
}
