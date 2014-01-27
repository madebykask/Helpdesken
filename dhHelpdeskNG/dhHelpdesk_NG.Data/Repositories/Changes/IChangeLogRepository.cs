namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IChangeLogRepository : IRepository<ChangeLogEntity>
    {
        List<LogOverview> FindOverviewsByHistoryIds(List<int> historyIds);
    }
}