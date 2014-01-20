namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectScheduleRepository : RepositoryBase<ProjectSchedule>, IProjectScheduleRepository
    {
        public ProjectScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewProjectScheduleDto newProjectSchedule)
        {
            var projectSchedule = this.MapFromDto(newProjectSchedule);
            this.DataContext.ProjectSchedules.Add(projectSchedule);
            this.InitializeAfterCommit(newProjectSchedule, projectSchedule);
        }

        public void Delete(int projectScheduleId)
        {
            var projectSchedule = this.DataContext.ProjectSchedules.Find(projectScheduleId);
            this.DataContext.ProjectSchedules.Remove(projectSchedule);
        }

        public void Update(NewProjectScheduleDto existingProjectSchedule)
        {
            var projectSchedule = this.DataContext.ProjectSchedules.Find(existingProjectSchedule.Id);

            projectSchedule.Project_Id = existingProjectSchedule.ProjectId;
            projectSchedule.ScheduleDate = existingProjectSchedule.StartDate;
            projectSchedule.CalculatedTime = existingProjectSchedule.Time;
            projectSchedule.Activity = existingProjectSchedule.Name;
            projectSchedule.Note = existingProjectSchedule.Description;
            projectSchedule.CaseNumber = existingProjectSchedule.CaseNumber;
            projectSchedule.FinishDate = existingProjectSchedule.FinishDate;
            projectSchedule.Pos = existingProjectSchedule.State;
        }

        public List<NewProjectSheduleOverview> Find(int projectId)
        {
            var projectshedules = this.DataContext.ProjectSchedules.Where(x => x.Project_Id == projectId).Select(this.MapToOverview).ToList();
            return projectshedules;
        }

        private ProjectSchedule MapFromDto(NewProjectScheduleDto newProjectSchedule)
        {
            return new ProjectSchedule
                       {
                           Activity = newProjectSchedule.Name,
                           CaseNumber = newProjectSchedule.CaseNumber,
                           IsActive = newProjectSchedule.State,
                           Note = newProjectSchedule.Description,
                           Pos = newProjectSchedule.Position,
                           Project_Id = newProjectSchedule.ProjectId,
                           ScheduleDate = newProjectSchedule.StartDate,
                           FinishDate = newProjectSchedule.FinishDate,
                           CalculatedTime = newProjectSchedule.Time,
                           User_Id = newProjectSchedule.UserId,
                       };
        }

        private NewProjectSheduleOverview MapToOverview(ProjectSchedule arg)
        {
            return new NewProjectSheduleOverview
                       {
                           Id = arg.Id,
                           Description = arg.Note,
                           Name = arg.Activity,
                           CaseNumber = arg.CaseNumber,
                           Position = arg.Pos,
                           ProjectId = arg.Project_Id,
                           State = arg.IsActive,
                           Time = arg.CalculatedTime,
                           FinishDate = arg.FinishDate,
                           StartDate = arg.ScheduleDate
                       };
        }
    }
}