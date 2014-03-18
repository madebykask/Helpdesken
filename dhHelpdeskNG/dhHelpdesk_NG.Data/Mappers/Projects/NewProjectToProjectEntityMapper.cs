namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

    public class NewProjectToProjectEntityMapper : INewBusinessModelToEntityMapper<NewProject, Project>
    {
        public Project Map(NewProject businessModel)
        {
            return new Project
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
                Description = businessModel.Description ?? string.Empty,
                Customer_Id = businessModel.CustomerId,

                CreatedDate = businessModel.StartDate ?? DateTime.Now,
                EndDate = businessModel.EndDate,
                IsActive = businessModel.IsActive,
                ProjectManager = businessModel.ProjectManagerId,

                // todo CreatedDate = businessModel.CreatedDate
            };
        }
    }
}
