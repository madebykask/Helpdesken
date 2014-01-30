namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

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
                
                // todo StartDate = businessModel.StartDate,
                EndDate = businessModel.EndDate,
                IsActive = businessModel.IsActive,
                ProjectManager = businessModel.ProjectManagerId,
                CreatedDate = businessModel.CreatedDate
            };
        }
    }
}
