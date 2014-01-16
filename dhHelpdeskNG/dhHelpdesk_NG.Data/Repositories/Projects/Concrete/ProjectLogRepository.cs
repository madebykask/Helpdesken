namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Projects;

    public class ProjectLogRepository : RepositoryBase<ProjectLog>, IProjectLogRepository
    {
        public ProjectLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}