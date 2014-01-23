namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectFileEntityToBusinessModelMapper : IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>
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