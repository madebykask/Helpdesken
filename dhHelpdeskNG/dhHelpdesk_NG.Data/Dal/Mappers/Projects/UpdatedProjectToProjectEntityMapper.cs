namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using System;

    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class UpdatedProjectToProjectEntityMapper : IBusinessModelToEntityMapper<UpdatedProject, Project>
    {
        public void Map(UpdatedProject businessModel, Project entity)
        {
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description ?? string.Empty;

            // todo 
            entity.CreatedDate = businessModel.StartDate ?? DateTime.Now;

            entity.EndDate = businessModel.EndDate;
            entity.IsActive = businessModel.IsActive;
            entity.ProjectManager = businessModel.ProjectManagerId;
            entity.ChangedDate = businessModel.ChangeDate;
        }
    }
}
