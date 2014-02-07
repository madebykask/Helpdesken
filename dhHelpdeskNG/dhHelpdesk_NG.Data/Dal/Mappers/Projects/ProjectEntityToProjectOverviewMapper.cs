namespace DH.Helpdesk.Dal.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectEntityToProjectOverviewMapper : IEntityToBusinessModelMapper<Project, ProjectOverview>
    {
        public ProjectOverview Map(Project entity)
        {
            var projectManagerName = entity.Manager == null
                                         ? string.Empty
                                         : string.Format("{0} {1}", entity.Manager.FirstName, entity.Manager.SurName);

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
                ProjectManagerName = projectManagerName
            };
        }
    }
}
