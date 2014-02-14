namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeDepartmentRepository : Repository, IChangeDepartmentRepository
    {
        public ChangeDepartmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<int> FindDepartmentIdsByChangeId(int changeId)
        {
            return
                this.DbContext.ChangeDepartments.Where(cd => cd.Change_Id == changeId)
                    .Select(cd => cd.Department_Id)
                    .ToList();
        }

        public void AddChangeDepartments(int changeId, List<int> departmentIds)
        {
            foreach (var departmentId in departmentIds)
            {
                var entity = new ChangeDepartmentEntity { Change_Id = changeId, Department_Id = departmentId };
                this.DbContext.ChangeDepartments.Add(entity);
            }
        }

        public void UpdateChangeDepartments(int changeId, List<int> departmentIds)
        {
            var existingRelations = this.DbContext.ChangeDepartments.Where(cd => cd.Change_Id == changeId).ToList();
            existingRelations.ForEach(r => this.DbContext.ChangeDepartments.Remove(r));

            foreach (var departmentId in departmentIds)
            {
                var newRelation = new ChangeDepartmentEntity { Change_Id = changeId, Department_Id = departmentId };
                this.DbContext.ChangeDepartments.Add(newRelation);
            }
        }
    }
}
