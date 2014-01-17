namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectLogRepository : IRepository<ProjectLog>
    {
        void Add(NewProjectLogDto newProject);

        void Delete(int projectId);

        List<NewProjectLogOverview> FindByProjectId(int projectId);
    }
}