namespace DH.Helpdesk.Dal.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

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
                           Note = businessModel.Description ?? string.Empty,
                           Pos = businessModel.Position,
                           Project_Id = businessModel.ProjectId,
                           ScheduleDate = businessModel.StartDate,
                           FinishDate = businessModel.FinishDate,
                           CalculatedTime = businessModel.Time,
                           User_Id = businessModel.UserId,
                           CreatedDate = businessModel.CreatedDate
                       };
        }
    }
}