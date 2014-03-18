namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

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
