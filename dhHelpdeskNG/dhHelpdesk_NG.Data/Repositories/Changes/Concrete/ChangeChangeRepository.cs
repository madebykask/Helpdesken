namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeChangeRepository : Repository, IChangeChangeRepository
    {
        public ChangeChangeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void UpdateRelatedChanges(int changeId, List<int> relatedChangeIds)
        {
            var existingRelations = this.DbContext.ChangeChanges.Where(cc => cc.Change_Id == changeId).ToList();
            existingRelations.ForEach(c => this.DbContext.ChangeChanges.Remove(c));

            foreach (var relatedChangeId in relatedChangeIds)
            {
                var entity = new ChangeChangeEntity { Change_Id = changeId, RelatedChange_Id = relatedChangeId };
                this.DbContext.ChangeChanges.Add(entity);
            }
        }

        public void AddRelatedChanges(int changeId, List<int> relatedChangeIds)
        {
            foreach (var relatedChangeId in relatedChangeIds)
            {
                var entity = new ChangeChangeEntity { Change_Id = changeId, RelatedChange_Id = relatedChangeId };
                this.DbContext.ChangeChanges.Add(entity);
            }
        }

        public void DeleteReferencesToChange(int changeId)
        {
            var references = this.DbContext.ChangeChanges.Where(cc => cc.RelatedChange_Id == changeId).ToList();
            references.ForEach(r => this.DbContext.ChangeChanges.Remove(r));
        }
    }
}
