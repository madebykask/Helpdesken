namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectCollaboratorRepository : IRepository<ProjectCollaborator>
    {
        void Add(ProjectCollaboratorDto newProjectCollaborator);

        void Delete(int projectId);

        List<ProjectCollaboratorOverview> Find(int projectId);
    }
}