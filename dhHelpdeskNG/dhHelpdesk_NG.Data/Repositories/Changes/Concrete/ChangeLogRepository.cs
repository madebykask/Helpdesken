namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public sealed class ChangeLogRepository : RepositoryBase<ChangeLogEntity>, IChangeLogRepository
    {
        public ChangeLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<LogOverview> FindByHistoryIds(List<int> historyIds)
        {
            var logs =
                this.DataContext.ChangeLogs.Where(l => historyIds.Contains(l.Id))
                    .Select(l => new { HistoryId = l.ChangeHistory_Id, Text = l.LogText })
                    .ToList();

            return logs.Select(l => new LogOverview(l.HistoryId, l.Text)).ToList();
        }
    }
}