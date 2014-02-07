namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region ACCOUNT

    public interface IAccountRepository : IRepository<Account>
    {
        Account GetAccount(int caseid);
    }

    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Account GetAccount(int caseid)
        {


            Account dup = (from du in this.DataContext.Set<Account>()
                           join d in this.DataContext.Cases on du.CaseNumber equals d.CaseNumber
                           where d.Id == caseid
                           select du).FirstOrDefault();


            return dup;
        }
    }

    #endregion

    #region ACCOUNTFIELDSETTING

    public interface IAccountFieldSettingsRepository : IRepository<AccountFieldSettings>
    {
    }

    public class AccountFieldSettingsRepository : RepositoryBase<AccountFieldSettings>, IAccountFieldSettingsRepository
    {
        public AccountFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ACCOUNTEMAILLOG

    public interface IAccountEMailLogRepository : IRepository<AccountEMailLog>
    {
    }

    public class AccountEMailLogRepository : RepositoryBase<AccountEMailLog>, IAccountEMailLogRepository
    {
        public AccountEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region ACCOUNTTYPE

    public interface IAccountTypeRepository : IRepository<AccountType>
    {
    }

    public class AccountTypeRepository : RepositoryBase<AccountType>, IAccountTypeRepository
    {
        public AccountTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
