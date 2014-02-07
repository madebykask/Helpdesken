namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeLogRepository : Repository, IChangeLogRepository
    {
        public ChangeLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void AddLog(NewLog log)
        {
            var entity = new ChangeLogEntity
                         {
                             ChangePart = (int)log.Subtopic,
                             Change_Id = log.ChangeId,
                             CreatedByUser_Id = log.RegisteredByUserId,
                             CreatedDate = log.CreatedDate,
                             LogText = log.Text
                         };

            this.DbContext.ChangeLogs.Add(entity);
            this.InitializeAfterCommit(log, entity);
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

        public List<Log> FindLogsByChangeIdAndSubtopic(int changeId, Subtopic subtopic, List<int> excludeIds)
        {
            var logs =
                this.DbContext.ChangeLogs.Where(
                    l =>
                        l.Change_Id == changeId && l.ChangePart == (int)subtopic && !excludeIds.Contains(l.Id)
                        && l.CreatedByUser_Id.HasValue && l.LogText != null);

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
                            RegisteredBy = u.FirstName + " " + u.SurName,
                            Text = l.LogText
                        }).ToList();

            return logsWithUserNames.Select(l => new Log(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
        }

        public void DeleteByIds(List<int> ids)
        {
            var logs = this.DbContext.ChangeLogs.Where(l => ids.Contains(l.Id)).ToList();
            logs.ForEach(l => this.DbContext.ChangeLogs.Remove(l));
        }
    }
}