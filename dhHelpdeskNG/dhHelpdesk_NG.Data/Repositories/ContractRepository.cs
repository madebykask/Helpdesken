using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region CONTRACT

    public interface IContractRepository : IRepository<Contract>
    {
    }

    public class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTCATEGORY

    public interface IContractCategoryRepository : IRepository<ContractCategory>
    {
    }

    public class ContractCategoryRepository : RepositoryBase<ContractCategory>, IContractCategoryRepository
    {
        public ContractCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTFIELDSETTINGS

    public interface IContractFieldSettingsRepository : IRepository<ContractFieldSettings>
    {
    }

    public class ContractFieldSettingsRepository : RepositoryBase<ContractFieldSettings>, IContractFieldSettingsRepository
    {
        public ContractFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTFILE

    public interface IContractFileRepository : IRepository<ContractFile>
    {
    }

    public class ContractFileRepository : RepositoryBase<ContractFile>, IContractFileRepository
    {
        public ContractFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTHISTORY

    public interface IContractHistoryRepository : IRepository<ContractHistory>
    {
    }

    public class ContractHistoryRepository : RepositoryBase<ContractHistory>, IContractHistoryRepository
    {
        public ContractHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CONTRACTLOG

    public interface IContractLogRepository : IRepository<ContractLog>
    {
    }

    public class ContractLogRepository : RepositoryBase<ContractLog>, IContractLogRepository
    {
        public ContractLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
