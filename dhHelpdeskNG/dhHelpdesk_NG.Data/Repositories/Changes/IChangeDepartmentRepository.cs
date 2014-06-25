namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeDepartmentRepository : INewRepository
    {
        void ResetChangeRelatedDepartments(int changeId);

        List<int> FindDepartmentIdsByChangeId(int changeId);

        List<ItemOverview> FindDepartmensByChangeId(int changeId);

        void AddChangeDepartments(int changeId, List<int> departmentIds);

        void UpdateChangeDepartments(int changeId, List<int> departmentIds);

        List<ChangeDepartmentEntity> FingByName(string name);
    }
}
