using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region USER

    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsers(int customerId);
        IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId);
        IList<LoggedOnUsersOnIndexPage> LoggedOnUsers();
        IList<UserLists> GetUserOnCases(int customer);
        User Login(string uId, string pwd);
        User GetUser(int userid);
        User GetUserByLogin(string IdName);
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<User> GetUsers(int customerId)
        {
            var query = from u in this.DataContext.Users
                        join cu in this.DataContext.CustomerUsers on u.Id equals cu.User_Id
                        where cu.Customer_Id == customerId
                        select u;

            return query;
        }

        public IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId)
        {
            var query = from wg in this.DataContext.WorkingGroups
                        join c in this.DataContext.Customers on wg.Customer_Id equals c.Id
                        join u in this.DataContext.Users on userId equals u.Id
                        group wg by new { wg.WorkingGroupName, userId, c.Name, u.Default_WorkingGroup_Id, wg.Id } into g
                        select new CustomerWorkingGroupForUser
                        {
                            WorkingGroupName = g.Key.WorkingGroupName,
                            User_Id = g.Key.userId,
                            CustomerName = g.Key.Name,
                            IsStandard = g.Key.Default_WorkingGroup_Id,
                            WorkingGroup_Id = g.Key.Id
                        };

            var queryList = query.OrderBy(x => x.CustomerName + x.WorkingGroupName).ToList();

            foreach (var q in queryList)
            {
                foreach (var uwg in this.DataContext.UserWorkingGroups.Where(x => x.User_Id == userId).ToList())
                {
                    if (q.WorkingGroup_Id == uwg.WorkingGroup_Id)
                    {
                        q.RoleToUWG = uwg.UserRole;
                    }
                }
            }

            return queryList;
        }

        public IList<LoggedOnUsersOnIndexPage> LoggedOnUsers()
        {
            var query = from u in this.DataContext.Users
                        join c in this.DataContext.Customers on u.Customer_Id equals c.Id
                        group u by new { u.FirstName, u.SurName, u.RegTime, u.Customer_Id, c.Name } into g    //, ca.CaseNumber
                        select new LoggedOnUsersOnIndexPage
                        {
                            Customer_Id = g.Key.Customer_Id,
                            UserFirstName = g.Key.FirstName,
                            UserLastName = g.Key.SurName,
                            CustomerName = g.Key.Name,
                            LoggedOnLastTime = g.Key.RegTime,
                        };

            return query.OrderByDescending(x => x.LoggedOnLastTime).ToList();
        }


        public User Login(string uId, string pwd)
        {
            var query = (from user in this.DataContext.Users
                         where user.UserID == uId && user.Password == pwd
                         select user).FirstOrDefault();

            return query;
        }

        public User GetUser(int userid)
        {
            return (from user in this.DataContext.Set<User>()
                    where user.Id == userid
                    select user).FirstOrDefault();
        }

        public IList<UserLists> GetUserOnCases(int customerId)
        {
            var query = from u in this.DataContext.Users
                        join ca in this.DataContext.Cases on u.Id equals ca.User_Id
                        where (ca.Customer_Id == customerId) 
                        group u by new { u.Id, u.FirstName, u.SurName } into g
                        select new UserLists
                        {
                            Id = g.Key.Id,
                            Name = g.Key.SurName + " " + g.Key.FirstName
                        };

            return query.OrderBy(x => x.Name).ToList();
        }

        public User GetUserByLogin(string IdName)
        {
            var query = (from user in this.DataContext.Users
                         where user.UserID == IdName && user.IsActive == 1
                         select user).SingleOrDefault();

            return query;
        }

    }

    #endregion

    #region USERGROUP

    public interface IUserGroupRepository : IRepository<UserGroup>
    {
    }

    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region USERROLE

    public interface IUserRoleRepository : IRepository<UserRole>
    {
    }

    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region USERPASSWORDHISTORY

    public interface IUsersPasswordHistoryRepository : IRepository<UsersPasswordHistory>
    {
    }

    public class UsersPasswordHistoryRepository : RepositoryBase<UsersPasswordHistory>, IUsersPasswordHistoryRepository
    {
        public UsersPasswordHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region USERWORKINGGROUP

    public interface IUserWorkingGroupRepository : IRepository<UserWorkingGroup>
    {
        UserWorkingGroup GetById(int workingGroupId, int userId);
    }

    public class UserWorkingGroupRepository : RepositoryBase<UserWorkingGroup>, IUserWorkingGroupRepository
    {
        public UserWorkingGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public UserWorkingGroup GetById(int workingGroupId, int userId)
        {
            return (from uwg in this.DataContext.Set<UserWorkingGroup>()
                    where uwg.WorkingGroup_Id == workingGroupId && uwg.User_Id == userId
                    select uwg).FirstOrDefault();
        }
    }

    #endregion
}
