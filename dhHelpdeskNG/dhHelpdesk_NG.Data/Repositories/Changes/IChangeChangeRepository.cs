namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;

    public interface IChangeChangeRepository : INewRepository
    {
        void UpdateRelatedChanges(int changeId, List<int> relatedChangeIds);

        void AddRelatedChanges(int changeId, List<int> relatedChangeIds);

        void DeleteReferencesToChange(int changeId);
    }
}
