namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public interface IChangeLogRepository : INewRepository
    {
        void AddLog(NewLog log);

        List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds);

        List<Log> FindLogsByChangeIdAndSubtopic(int changeId, Subtopic subtopic, List<int> excludeIds);

        void DeleteByIds(List<int> ids);
    }
}