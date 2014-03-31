namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeLogRepository : INewRepository
    {
        List<Log> FindLogsByChangeId(int changeId);

        List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds);

        List<Log> FindLogsExcludeSpecified(int changeId, Subtopic subtopic, List<int> excludeLogIds);

        void DeleteByIds(List<int> logIds);

        void AddManualLog(NewLog log);
    }
}