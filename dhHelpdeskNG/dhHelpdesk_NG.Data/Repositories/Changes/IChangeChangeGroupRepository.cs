namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Dal.Dal;

    public interface IChangeChangeGroupRepository : INewRepository
    {
        List<int> FindProcessIdsByChangeId(int changeId);

        void AddChangeProcesses(int changeId, List<int> processIds);

        void UpdateChangeProcesses(int changeId, List<int> processIds);
    }
}
