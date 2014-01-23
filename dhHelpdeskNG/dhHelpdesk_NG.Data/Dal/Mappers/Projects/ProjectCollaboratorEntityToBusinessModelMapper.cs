namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectCollaboratorEntityToBusinessModelMapper : IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>
    {
        public ProjectCollaboratorOverview Map(ProjectCollaborator entity)
        {
            return new ProjectCollaboratorOverview
                       {
                           Id = entity.Id,
                           ProjectId = entity.Project_Id,
                           UserId = entity.User_Id,
                           UserName = string.Format("{0} {1}", entity.User.FirstName, entity.User.SurName),
                       };
        }
    }
}