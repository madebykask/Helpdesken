namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectEntityToProjectOverviewMapper : IEntityToBusinessModelMapper<Project, ProjectOverview>
    {
        public ProjectOverview Map(Project entity)
        {
            return new ProjectOverview
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CustomerId = entity.Customer_Id,
                
                // todo 
                StartDate = entity.CreatedDate,
                
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                ProjectManagerId = entity.ProjectManager,
                ProjectManagerName = (entity.Manager == null? string.Empty : entity.Manager.FirstName),
                ProjectManagerSurName =  (entity.Manager == null? string.Empty : entity.Manager.SurName)
            };
        }
    }
}
