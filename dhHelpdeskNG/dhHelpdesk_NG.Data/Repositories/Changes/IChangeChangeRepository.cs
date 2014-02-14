namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Dal.Dal;

    public interface IChangeChangeRepository : INewRepository
    {
        void UpdateRelatedChanges(int changeId, List<int> relatedChangeIds);

        void AddRelatedChanges(int changeId, List<int> relatedChangeIds);

        void DeleteReferencesToChange(int changeId);

        List<int> FindRelatedChangeIdsByChangeId(int changeId);
    }
}
