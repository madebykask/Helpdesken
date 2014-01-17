namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectRepository : IRepository<Project>
    {
        void Add(NewProjectDto newProject);

        void Delete(int projectId);

        void Update(NewProjectDto existingProject);

        NewProjectOverview FindById(int projectId);

        List<NewProjectOverview> Find(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike);
    }
}