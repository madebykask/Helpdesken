namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class ProblemLogRepository : RepositoryBase<ProblemLog>, IProblemLogRepository
    {
        public ProblemLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}