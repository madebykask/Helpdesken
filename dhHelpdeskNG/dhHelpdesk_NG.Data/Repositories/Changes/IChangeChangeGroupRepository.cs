namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeChangeGroupRepository : INewRepository
    {
        void ResetChangeRelatedProcesses(int changeId);

        List<int> FindProcessIdsByChangeId(int changeId);

        List<ItemOverview> FindProcessesByChangeId(int changeId);

        void AddChangeProcesses(int changeId, List<int> processIds);

        void UpdateChangeProcesses(int changeId, List<int> processIds);
    }
}
