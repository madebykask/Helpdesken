namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeChangeGroupRepository : Repository, IChangeChangeGroupRepository
    {
        public ChangeChangeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetChangeRelatedProcesses(int changeId)
        {
            var relations = this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == changeId).ToList();
            relations.ForEach(r => this.DbContext.ChangeChangeGroups.Remove(r));
        }

        public List<int> FindProcessIdsByChangeId(int changeId)
        {
            return
                this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == changeId)
                    .Select(cg => cg.ChangeGroup_Id)
                    .ToList();
        }

        public List<ItemOverview> FindProcessesByChangeId(int changeId)
        {
            var entities = this.DbContext.ChangeChangeGroups
                    .Where(cg => cg.Change_Id == changeId)
                    .Select(cg => new
                                {
                                    Value = cg.ChangeGroup_Id.ToString(),
                                    Name = cg.ChangeGroup.ChangeGroup  
                                })
                    .OrderBy(cg => cg.Name)
                    .ToList();
            return entities
                    .Select(cg => new ItemOverview(cg.Name, cg.Value))
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
