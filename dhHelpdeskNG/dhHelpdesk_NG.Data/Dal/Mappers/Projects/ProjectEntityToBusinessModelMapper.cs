namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectEntityToBusinessModelMapper : IEntityToBusinessModelMapper<Project, ProjectOverview>
    {
        public ProjectOverview Map(Project entity)
        {
            return new ProjectOverview
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CustomerId = entity.Customer_Id,
                FinishDate = entity.FinishDate,
                IsActive = entity.IsActive,
                ProjectManagerId = entity.ProjectManager
            };
        }
    }
}
