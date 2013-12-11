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

    public interface IEMailLogRepository : IRepository<EMailLog>
    {
    }

    public class EMailLogRepository : RepositoryBase<EMailLog>, IEMailLogRepository
    {
        public EMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
