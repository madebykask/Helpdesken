namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class NewProjectScheduleToProjectScheduleEntityMapper : INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>
    {
        public ProjectSchedule Map(NewProjectSchedule businessModel)
        {
            return new ProjectSchedule
                       {
                           Id = businessModel.Id,
                           Activity = businessModel.Name,
                           CaseNumber = businessModel.CaseNumber,
                           IsActive = businessModel.State,
                           Note = businessModel.Description,
                           Pos = businessModel.Position,
                           Project_Id = businessModel.ProjectId,
                           ScheduleDate = businessModel.StartDate,
                           FinishDate = businessModel.FinishDate,
                           CalculatedTime = businessModel.Time,
                           User_Id = businessModel.UserId,
                       };
        }
    }
}