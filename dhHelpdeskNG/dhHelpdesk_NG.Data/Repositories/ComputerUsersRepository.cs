using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Enums.Notifiers;

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

