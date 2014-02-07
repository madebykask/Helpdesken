namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class EmailGroupEmailRepository : Repository, IEmailGroupEmailRepository
    {
        public EmailGroupEmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemWithEmails> FindEmailGroupsEmails(List<int> emailGroupIds)
        {
            var emailGroups =
                this.DbContext.EMailGroups.Where(g => emailGroupIds.Contains(g.Id))
                    .Select(g => new { g.Id, Emails = g.Members })
                    .ToList();

            return
                emailGroups.Select(
                    g =>
                        new ItemWithEmails(
                            g.Id,
                            g.Emails.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList())).ToList();
        }
    }
}
