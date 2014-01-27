namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IChangeEmailLogRepository : IRepository<ChangeEmailLogEntity>
    {
        void DeleteByHistoryId(int historyId);

        List<EmailLogOverview> FindOverviewsByHistoryIds(List<int> historyIds);
    }
}