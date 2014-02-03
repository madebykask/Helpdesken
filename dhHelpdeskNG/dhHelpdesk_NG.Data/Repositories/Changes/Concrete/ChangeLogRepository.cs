namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class ChangeLogRepository : Repository, IChangeLogRepository
    {
        public ChangeLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds)
        {
            var logs =
                this.DbContext.ChangeLogs.Where(
                    l => l.ChangeHistory_Id.HasValue && historyIds.Contains(l.ChangeHistory_Id.Value))
                    .Select(l => new { HistoryId = l.ChangeHistory_Id, Text = l.LogText })
                    .ToList();

            return logs.Select(l => new LogOverview(l.HistoryId.Value, l.Text)).ToList();
        }

        public List<Log> FindLogsByChangeIdAndSubtopic(int changeId, Subtopic subtopic)
        {
            var logs =
                this.DbContext.ChangeLogs.Where(
                    l =>
                        l.Change_Id == changeId && l.ChangePart == (int)subtopic && l.CreatedByUser_Id.HasValue
                        && !string.IsNullOrEmpty(l.LogText));

            var logsWithUserNames =
                logs.Join(
                    this.DbContext.Users,
                    l => l.CreatedByUser_Id,
                    u => u.Id,
                    (l, u) =>
                        new
                        {
                            l.Id,
                            DateAndTime = l.CreatedDate,
                            RegisteredBy = u.FirstName + u.SurName,
                            Text = l.LogText
                        }).ToList();

            return logsWithUserNames.Select(l => new Log(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
        }
    }
}