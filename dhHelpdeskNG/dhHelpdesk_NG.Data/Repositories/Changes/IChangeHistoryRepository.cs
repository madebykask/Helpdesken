namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeHistoryRepository : INewRepository
    {
        void DeleteByChangeId(int changeId);

        List<int> FindIdsByChangeId(int changeId);

        List<History> FindHistoriesByChangeId(int changeId);
        
        void AddChangeToHistory(UpdatedChange change);
    }
}
