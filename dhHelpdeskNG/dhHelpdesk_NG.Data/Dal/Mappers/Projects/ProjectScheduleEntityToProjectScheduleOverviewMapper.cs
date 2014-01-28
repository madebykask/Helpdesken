namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectScheduleEntityToProjectScheduleOverviewMapper : IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>
    {
        public ProjectScheduleOverview Map(ProjectSchedule entity)
        {
            return new ProjectScheduleOverview
                       {
                           Id = entity.Id,
                           Description = entity.Note ?? string.Empty,
                           Name = entity.Activity,
                           CaseNumber = entity.CaseNumber,
                           Position = entity.Pos,
                           ProjectId = entity.Project_Id,
                           State = entity.IsActive,
                           Time = entity.CalculatedTime,
                           FinishDate = entity.FinishDate,
                           StartDate = entity.ScheduleDate,
                           UserId = entity.User_Id
                       };
        }
    }
}