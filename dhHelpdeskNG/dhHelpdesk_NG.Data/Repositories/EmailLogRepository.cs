namespace dhHelpdesk_NG.Data.Repositories
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}