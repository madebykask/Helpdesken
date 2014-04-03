namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeHistoryRepository : INewRepository
    {
        void DeleteByChangeId(int changeId);

        List<int> FindIdsByChangeId(int changeId);

        List<HistoryOverview> FindHistoriesByChangeId(int changeId);
        
        void AddHistory(History history);
    }
}
