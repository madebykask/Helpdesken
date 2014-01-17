namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public interface IProjectScheduleRepository : IRepository<ProjectSchedule>
    {
        void Add(NewProjectScheduleDto newProject);

        void Delete(int projectId);

        void Update(NewProjectScheduleDto existingProject);

        List<NewProjectScheduleDto> Find(int projectId);
    }
}