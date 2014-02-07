namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeLogRepository : INewRepository
    {
        void AddLog(NewLog log);

        List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds);

        List<Log> FindLogsByChangeIdAndSubtopic(int changeId, Subtopic subtopic, List<int> excludeIds);

        void DeleteByIds(List<int> ids);
    }
}