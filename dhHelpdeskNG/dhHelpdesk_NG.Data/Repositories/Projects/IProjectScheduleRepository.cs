namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectScheduleRepository : IRepository<ProjectSchedule>
    {
        void Add(NewProjectScheduleDto newProjectSchedule);

        void Delete(int projectScheduleId);

        void Update(NewProjectScheduleDto existingProjectSchedule);

        List<NewProjectSheduleOverview> Find(int projectId);
    }
}