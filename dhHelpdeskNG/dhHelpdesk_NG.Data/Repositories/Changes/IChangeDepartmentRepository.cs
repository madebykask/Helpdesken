namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Dal.Dal;

    public interface IChangeDepartmentRepository : INewRepository
    {
        List<int> FindDepartmentIdsByChangeId(int changeId);

        void AddChangeDepartments(int changeId, List<int> departmentIds);

        void UpdateChangeDepartments(int changeId, List<int> departmentIds);
    }
}
