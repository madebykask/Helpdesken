namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeLogRepository : INewRepository
    {
        void UpdateLogEmailLogId(int logId, int emailLogId);

        List<Log> FindLogsByChangeId(int changeId);

        List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds);

        List<Log> FindLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds);

        void DeleteByChangeId(int changeId);

        void DeleteByIds(List<int> logIds);

        void AddManualLog(ManualLog log);

        void AddLogs(List<ManualLog> logs);
    }
}