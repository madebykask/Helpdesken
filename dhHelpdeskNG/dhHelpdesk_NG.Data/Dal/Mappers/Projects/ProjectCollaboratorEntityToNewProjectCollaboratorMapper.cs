namespace DH.Helpdesk.Dal.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectCollaboratorEntityToNewProjectCollaboratorMapper : IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>
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