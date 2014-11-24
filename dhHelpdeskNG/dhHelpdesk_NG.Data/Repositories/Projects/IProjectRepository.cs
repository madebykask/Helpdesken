namespace DH.Helpdesk.Dal.Repositories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums;

    public interface IProjectRepository : INewRepository
    {
        void Add(NewProject businessModel);

        void Delete(int id);

        void Update(UpdatedProject businessModel);

        ProjectOverview FindById(int projectId);

        List<ProjectOverview> Find(int customerId);

        List<ProjectOverview> Find(
            int customerId,
            EntityStatus entityStatus,
            int? projectManagerId,
            string projectNameLike,
            SortField sortField);
    }
}