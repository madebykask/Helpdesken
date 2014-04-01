namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeEmailLogRepository : INewRepository
    {
        void DeleteByHistoryIds(List<int> historyIds);

        List<EmailLogOverview> FindOverviewsByHistoryIds(List<int> historyIds);

        void AddEmailLog(EmailLog log);
    }
}