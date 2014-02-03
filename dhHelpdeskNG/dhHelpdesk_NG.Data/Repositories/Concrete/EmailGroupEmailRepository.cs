namespace dhHelpdesk_NG.Data.Repositories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.DTO.DTOs;

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
