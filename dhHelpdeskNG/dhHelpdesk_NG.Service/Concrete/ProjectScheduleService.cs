namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectScheduleService : IProjectScheduleService
    {
        private readonly IProjectScheduleRepository projectScheduleRepository;

        public ProjectScheduleService(IProjectScheduleRepository projectScheduleRepository)
        {
            this.projectScheduleRepository = projectScheduleRepository;
        }

        public void AddSchedule(NewProjectSchedule schedule)
        {
            this.projectScheduleRepository.Add(schedule);
            this.projectScheduleRepository.Commit();
        }

        public void DeleteSchedule(int id)
        {
            this.projectScheduleRepository.Delete(id);
            this.projectScheduleRepository.Commit();
        }

        public void UpdateSchedule(UpdatedProjectSchedule schedule)
        {
            this.projectScheduleRepository.Update(schedule);
            this.projectScheduleRepository.Commit();
        }

        public void UpdateSchedule(List<UpdatedProjectSchedule> schedules)
        {
            this.projectScheduleRepository.Update(schedules);
            this.projectScheduleRepository.Commit();
        }

        public List<ProjectScheduleOverview> GetProjectSchedules(int projectId)
        {
            var schedules = this.projectScheduleRepository.Find(projectId);
            return schedules;
        }
    }
}