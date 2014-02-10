namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;

    #region COMPUTERUSERCUSTOMERUSER

    public interface IComputerUserCustomerUserGroupRepository : IRepository<ComputerUserCustomerUserGroupRepository>
    {
    }

    public class ComputerUserCustomerUserGroupRepository : RepositoryBase<ComputerUserCustomerUserGroupRepository>, IComputerUserCustomerUserGroupRepository
    {
        public ComputerUserCustomerUserGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERUSERLOG

    public interface IComputerUserLogRepository : IRepository<ComputerUserLog>
    {
    }

    public class ComputerUserLogRepository : RepositoryBase<ComputerUserLog>, IComputerUserLogRepository
    {
        public ComputerUserLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region COMPUTERUSERBLACKLIST

    public interface IComputerUsersBlackListRepository : IRepository<ComputerUsersBlackList>
    {
    }

    public class ComputerUsersBlackListRepository : RepositoryBase<ComputerUsersBlackList>, IComputerUsersBlackListRepository
    {
        public ComputerUsersBlackListRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}

