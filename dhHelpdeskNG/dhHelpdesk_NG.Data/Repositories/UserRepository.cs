using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    #region USER

    public interface IUserRepository : IRepository<User>
    {
        List<ItemOverviewDto> FindActiveOverviews(int customerId);

        IEnumerable<User> GetUsers(int customerId);
        IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId);
        IList<LoggedOnUsersOnIndexPage> LoggedOnUsers();
        IList<UserLists> GetUserOnCases(int customer);
        IList<User> GetUsersForUserSettingList(int statusId, UserSearch searchUser);
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

        private IQueryable<User> FindByCustomerId(int customerId)
        {
            return this.DataContext.Users.Where(u => u.Customer_Id == customerId);
        }

        public List<ItemOverviewDto> FindActiveOverviews(int customerId)
        {
            var users = this.FindByCustomerId(customerId).Where(u => u.IsActive != 0);
            var overviews = users.Select(u => new { Name = u.FirstName + u.SurName, Value = u.Id }).ToList();

            return
                overviews.Select(
                    o => new ItemOverviewDto { Name = o.Name, Value = o.Value.ToString(CultureInfo.InvariantCulture) })
                         .ToList();
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

            var query = from cu in this.DataContext.CustomerUsers.Where(x=>x.User_Id == userId)
                        join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                        join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                        join u in this.DataContext.Users on userId equals u.Id
                        from uwg in this.DataContext.UserWorkingGroups.Where(x=>x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
                        group uwg by new { wg.WorkingGroupName, userId, c.Name, wg.Id, u.Default_WorkingGroup_Id, uwg.UserRole } into g
                        select new CustomerWorkingGroupForUser
                        {
                            WorkingGroupName = g.Key.WorkingGroupName,
                            User_Id = userId,
                            CustomerName = g.Key.Name,
                            IsStandard = g.Key.Default_WorkingGroup_Id,
                            WorkingGroup_Id = g.Key.Id,
                            RoleToUWG = g.Key.UserRole == null ? 0 : g.Key.UserRole
                        };      

            var queryList = query.OrderBy(x => x.CustomerName + x.WorkingGroupName).ToList();

            return queryList;

        }

        public IList<User> GetUsersForUserSettingList(int statusId, UserSearch searchUser)
        {
            var query = from u in this.DataContext.Users
                        join cu in this.DataContext.CustomerUsers on u.Id equals cu.User_Id
                        where cu.Customer_Id == searchUser.CustomerId
                        select u;

            if (statusId == 2)
                query = query.Where(x => x.IsActive == 0);
            else if (statusId == 1)
                query = query.Where(x => x.IsActive == 1);
            else if (statusId == 3)
                query = query.Where(x => x.IsActive == 1 || x.IsActive == 0);


            if (!string.IsNullOrWhiteSpace(searchUser.SearchUs))
            {
                string s = searchUser.SearchUs.ToLower();
                query = query.Where(x => x.UserID.ToLower().Contains(s)
                    || x.ArticleNumber.ToLower().Contains(s)
                    || x.CellPhone.ToLower().Contains(s)
                    || x.Email.ToLower().Contains(s)
                    || x.FirstName.ToLower().Contains(s)
                    || x.Phone.ToLower().Contains(s)
                    || x.SurName.ToLower().Contains(s)
                    || x.UserGroup.Name.ToLower().Contains(s));
            }

            return query.OrderBy(x => x.UserID).ToList();
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
