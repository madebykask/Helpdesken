using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region EMAILGROUP

    public interface IEMailGroupRepository : IRepository<EMailGroup>
    {
    }

    public class EMailGroupRepository : RepositoryBase<EMailGroup>, IEMailGroupRepository
    {
        public EMailGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region EMAILLOG

    public interface IEmailLogRepository : IRepository<EmailLog>
    {
    }

    public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
