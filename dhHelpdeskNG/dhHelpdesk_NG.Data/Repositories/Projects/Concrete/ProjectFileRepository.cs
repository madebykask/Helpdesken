namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Projects;

    public class ProjectFileRepository : RepositoryBase<ProjectFile>, IProjectFileRepository
    {
        public ProjectFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}