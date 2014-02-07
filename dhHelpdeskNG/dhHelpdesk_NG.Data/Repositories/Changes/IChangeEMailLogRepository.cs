namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeEmailLogRepository : IRepository<ChangeEmailLogEntity>
    {
        void DeleteByHistoryId(int historyId);

        List<EmailLogOverview> FindOverviewsByHistoryIds(List<int> historyIds);
    }
}