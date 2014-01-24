namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using System;

    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class UpdatedProjectScheduleToProjectScheduleEntityMapper : IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>
    {
        public void Map(UpdatedProjectSchedule businessModel, ProjectSchedule entity)
        {
            entity.Activity = businessModel.Name;
            entity.CaseNumber = businessModel.CaseNumber;
            entity.IsActive = businessModel.State;
            entity.Note = businessModel.Description;
            entity.Pos = businessModel.Position;
            entity.ScheduleDate = businessModel.StartDate;
            entity.FinishDate = businessModel.FinishDate;
            entity.CalculatedTime = businessModel.Time;
            entity.User_Id = businessModel.UserId;
            entity.ChangedDate = businessModel.ChangeDate;
        }
    }
}