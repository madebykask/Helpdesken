namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectCollaboratorRepository : RepositoryDecoratorBase<ProjectCollaborator, ProjectCollaboratorDto>, IProjectCollaboratorRepository
    {
        public ProjectCollaboratorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public override ProjectCollaborator MapFromDto(ProjectCollaboratorDto newProjectCollaborator)
        {
            throw new global::System.NotImplementedException();
        }

        public List<ProjectCollaboratorOverview> Find(int projectId)
        {
            throw new global::System.NotImplementedException();
        }
    }
}