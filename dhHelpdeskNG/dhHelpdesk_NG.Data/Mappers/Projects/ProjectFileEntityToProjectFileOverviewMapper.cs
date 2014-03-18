namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectFileEntityToProjectFileOverviewMapper : IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>
    {
        public ProjectFileOverview Map(ProjectFile entity)
        {
            return new ProjectFileOverview
                       {
                           ProjectId = entity.Project_Id,
                           Name = entity.FileName
                       };
        }
    }
}