namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeEmailLogRepository : Repository, IChangeEmailLogRepository
    {
        public ChangeEmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByHistoryIds(List<int> historyIds)
        {
            var logs = this.DbContext.ChangeEMailLogs.Where(l => historyIds.Contains(l.ChangeHistory_Id)).ToList();
            logs.ForEach(l => this.DbContext.ChangeEMailLogs.Remove(l));
        }

        public List<EmailLogOverview> FindOverviewsByHistoryIds(List<int> historyIds)
        {
            var logs =
                this.DbContext.ChangeEMailLogs.Where(l => historyIds.Contains(l.ChangeHistory_Id))
                    .Select(l => new { l.ChangeHistory_Id, l.EMailAddress })
                    .ToList();

            return logs.Select(l => new EmailLogOverview(l.ChangeHistory_Id, l.EMailAddress)).ToList();
        }

        public void AddEmailLog(EmailLog log)
        {
            var entity = new ChangeEmailLogEntity
                         {
                             ChangeHistory_Id = log.HistoryId,
                             CreatedDate = log.CreatedDateAndTime,
                             EMailAddress = string.Join(";", log.Emails),
                             MailID = log.MailId,
                             MessageId = log.MessageId
                         };

            this.DbContext.ChangeEMailLogs.Add(entity);
            this.InitializeAfterCommit(log, entity);
        }
    }
}