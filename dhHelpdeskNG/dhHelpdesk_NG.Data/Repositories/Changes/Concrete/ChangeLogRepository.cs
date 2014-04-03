namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
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

        public void UpdateLogEmailLogId(int logId, int emailLogId)
        {
            var log = this.DbContext.ChangeLogs.Find(logId);
            log.ChangeEMailLog_Id = emailLogId;
        }

        public List<Log> FindLogsByChangeId(int changeId)
        {
            var changeLogs = this.DbContext.ChangeLogs.Where(l => l.Change_Id == changeId);

            var logs =
                changeLogs.Join(
                    this.DbContext.Users,
                    l => l.CreatedByUser_Id,
                    u => u.Id,
                    (l, u) => new { l.Id, l.ChangePart, l.CreatedDate, u.FirstName, u.SurName, l.LogText }).ToList();

            return
                logs.Select(
                    l =>
                        new Log(
                            l.Id,
                            (Subtopic)l.ChangePart,
                            l.CreatedDate,
                            new UserName(l.FirstName, l.SurName),
                            l.LogText)).ToList();
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

        public List<Log> FindLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds)
        {
            var logs =
                this.DbContext.ChangeLogs.Where(
                    l =>
                        l.Change_Id == changeId && l.ChangePart == (int)subtopic && !excludeLogIds.Contains(l.Id)
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
                            RegisteredByFirstName = u.FirstName,
                            RegisteredByLastName = u.SurName,
                            Text = l.LogText
                        }).ToList();

            return
                logsWithUserNames.Select(
                    l =>
                        new Log(
                            l.Id,
                            subtopic,
                            l.DateAndTime,
                            new UserName(l.RegisteredByFirstName, l.RegisteredByLastName),
                            l.Text)).ToList();
        }

        public void DeleteByIds(List<int> logIds)
        {
            var logs = this.DbContext.ChangeLogs.Where(l => logIds.Contains(l.Id)).ToList();
            logs.ForEach(l => this.DbContext.ChangeLogs.Remove(l));
        }

        public void AddManualLog(ManualLog log)
        {
            var entity = new ChangeLogEntity
                         {
                             ChangeEMailLog_Id = log.ChangeEmailLogId,
                             ChangeHistory_Id = log.ChangeHistoryId,
                             ChangePart = (int)log.Subtopic,
                             Change_Id = log.ChangeId,
                             CreatedByUser_Id = log.CreatedByUserId,
                             CreatedDate = log.CreatedDateAndTime,
                             LogText = log.Text
                         };

            this.DbContext.ChangeLogs.Add(entity);
        }

        public void AddLogs(List<ManualLog> logs)
        {
            foreach (var log in logs)
            {
                var entity = new ChangeLogEntity
                             {
                                 ChangeHistory_Id = log.ChangeHistoryId,
                                 ChangePart = (int)log.Subtopic,
                                 Change_Id = log.ChangeId,
                                 CreatedByUser_Id = log.CreatedByUserId,
                                 CreatedDate = log.CreatedDateAndTime,
                                 LogText = log.Text
                             };

                this.DbContext.ChangeLogs.Add(entity);
                this.InitializeAfterCommit(log, entity);
            }
        }
    }
}