namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public sealed class ChangeEmailLogRepository : RepositoryBase<ChangeEmailLogEntity>, IChangeEmailLogRepository
    {
        public ChangeEmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByHistoryId(int historyId)
        {
            var logs = this.DataContext.ChangeEMailLogs.Where(l => l.ChangeHistory_Id == historyId).ToList();
            logs.ForEach(l => this.DataContext.ChangeEMailLogs.Remove(l));
        }

        public List<EmailLogOverview> FindOverviewsByHistoryIds(List<int> historyIds)
        {
            var logs =
                this.DataContext.ChangeEMailLogs.Where(l => historyIds.Contains(l.ChangeHistory_Id))
                    .Select(l => new { HistoryId = l.ChangeHistory_Id, Email = l.EMailAddress })
                    .ToList();

            return logs.Select(l => new EmailLogOverview(l.HistoryId, l.Email)).ToList();
        }
    }
}