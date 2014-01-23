namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectRepository : INewRepository
    {
        void Add(NewProject businessModel);

        void Delete(int id);

        void Update(UpdatedProject businessModel);

        ProjectOverview FindById(int projectId);

        List<ProjectOverview> Find(int customerId);

        List<ProjectOverview> Find(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike);
    }
}