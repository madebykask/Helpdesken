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
                Description = businessModel.Description,
                Customer_Id = businessModel.CustomerId,
                FinishDate = businessModel.FinishDate,
                IsActive = businessModel.IsActive,
                ProjectManager = businessModel.ProjectManagerId
            };
        }
    }
}
