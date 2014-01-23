namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectScheduleService
    {
        void AddSchedule(NewProjectSchedule schedule);

        void DeleteSchedule(int id);

        void UpdateSchedule(UpdatedProjectSchedule schedule);

        void UpdateSchedule(List<UpdatedProjectSchedule> schedules);

        List<ProjectScheduleOverview> GetProjectSchedules(int projectId);
    }
}