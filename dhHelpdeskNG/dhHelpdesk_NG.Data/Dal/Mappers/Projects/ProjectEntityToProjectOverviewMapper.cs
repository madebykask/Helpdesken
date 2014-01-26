namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

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
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                ProjectManagerId = entity.ProjectManager,
                ProjectManagerName = projectManagerName
            };
        }
    }
}
