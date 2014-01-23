namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using System;

    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class ProjectEntityFromBusinessModelChanger : IEntityChangerFromBusinessModel<NewProjectDto, Project>
    {
        public void Map(NewProjectDto businessModel, Project entity)
        {
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.Customer_Id = businessModel.CustomerId;
            entity.FinishDate = businessModel.FinishDate;
            entity.IsActive = businessModel.IsActive;
            entity.ProjectManager = businessModel.ProjectManagerId;
            entity.ChangedDate = DateTime.Now;
        }
    }
}
