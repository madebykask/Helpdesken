namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

    public class UpdatedProjectScheduleToProjectScheduleEntityMapper : IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>
    {
        public void Map(UpdatedProjectSchedule businessModel, ProjectSchedule entity)
        {
            entity.Activity = businessModel.Name;
            entity.CaseNumber = businessModel.CaseNumber;
            entity.IsActive = businessModel.State;
            entity.Note = businessModel.Description ?? string.Empty;
            entity.Pos = businessModel.Position;
            entity.ScheduleDate = businessModel.StartDate;
            entity.FinishDate = businessModel.FinishDate;
            entity.CalculatedTime = businessModel.Time;
            entity.User_Id = businessModel.UserId;
            entity.ChangedDate = businessModel.ChangeDate;
        }
    }
}