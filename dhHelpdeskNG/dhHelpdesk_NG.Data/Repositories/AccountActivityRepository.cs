namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region ACCOUNTACTIVITY

    public interface IAccountActivityRepository : IRepository<AccountActivity>
    {
        // expandable ....
    }

    public class AccountActivityRepository : RepositoryBase<AccountActivity>, IAccountActivityRepository
    {
        public AccountActivityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ACCOUNTACTIVITYGROUP

    public interface IAccountActivityGroupRepository : IRepository<AccountActivityGroup>
    {
        // expandable ....
    }

    public class AccountActivityGroupRepository : RepositoryBase<AccountActivityGroup>, IAccountActivityGroupRepository
    {
        public AccountActivityGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
