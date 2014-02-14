namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeChangeGroupRepository : Repository, IChangeChangeGroupRepository
    {
        public ChangeChangeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<int> FindProcessIdsByChangeId(int changeId)
        {
            return
                this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == changeId)
                    .Select(cg => cg.ChangeGroup_Id)
                    .ToList();
        }

        public void AddChangeProcesses(int changeId, List<int> processIds)
        {
            foreach (var processId in processIds)
            {
                var entity = new ChangeChangeGroupEntity { Change_Id = changeId, ChangeGroup_Id = processId };
                this.DbContext.ChangeChangeGroups.Add(entity);
            }
        }

        public void UpdateChangeProcesses(int changeId, List<int> processIds)
        {
            var existingRelations = this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == changeId).ToList();
            existingRelations.ForEach(r => this.DbContext.ChangeChangeGroups.Remove(r));

            foreach (var processId in processIds)
            {
                var newRelation = new ChangeChangeGroupEntity { Change_Id = changeId, ChangeGroup_Id = processId };
                this.DbContext.ChangeChangeGroups.Add(newRelation);
            }
        }
    }
}
