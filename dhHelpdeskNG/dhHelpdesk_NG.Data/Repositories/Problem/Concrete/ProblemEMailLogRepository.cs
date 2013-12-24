namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class ProblemEMailLogRepository : RepositoryBase<ProblemEMailLog>, IProblemEMailLogRepository
    {
        public ProblemEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}