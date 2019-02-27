using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Common.Linq;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Users;


namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System.Threading.Tasks;

    #region USER

    public interface IUserRepository : IRepository<User>
    {
        List<ItemOverview> FindActiveUsersIncludeEmails(int customerId);

        UserName GetUserNameById(int userId);

        List<ItemOverview> FindActiveOverviews(int customerId);

        List<ItemWithEmail> FindUsersEmails(List<int> userIds, bool isActive = false);

        IQueryable<User> GetUsers(int customerId);
        IQueryable<User> GetUsersByUserGroup(int customerId);
        IQueryable<User> GetUsersForWorkingGroupQuery(int workingGroupId, int? customerId = null, bool requireMemberOfGroup = false);

        IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId);
        IList<CustomerWorkingGroupForUser> GetWorkinggroupsForUserAndCustomer(int userId, int customerId);
        IList<LoggedOnUsersOnIndexPage> LoggedOnUsers();

        IList<User> GetUsersForUserSettingList(UserSearch searchUser);
        IList<User> GetUsersForUserSettingListByUserGroup(UserSearch searchUser);
        UserLoginInfo GetUserLoginInfo(string userId);
        Task<UserOverview> GetByUserIdAsync(string userId, string passw);
        DateTime GetPasswordChangedDate(int id);

        User GetUserForCopy(int id);
        CustomerUserInfo GetUserInfo(int userId); //basic information - good perf
        UserOverview GetUser(int userid); // full information
        Task<UserOverview> GetUserAsync(int userId);

        IList<UserLists> GetUserOnCases(int customerId, bool isTakeOnlyActive = false);

        UserOverview GetUserByLogin(string IdName, int? customerId = null);

        UserName GetUserName(int userId);

        ItemOverview FindActiveOverview(int userId);

        List<ItemOverview> FindUsersWithPermissionsForCustomers(int[] customers);
        
        IQueryable<User> FindUsersByUserId(string userId);
        IEnumerable<User> FindUsersByName(string name);

        bool UserHasActiveCase(int customerId, int userId, List<int> workingGroupIds);

        int? GetUserDefaultWorkingGroupId(int userId, int customerId);

        WorkingGroupEntity GetUserDefaultWorkingGroup(int userId, int customerId);

        string GetUserTimeZoneId(int userId);
    }

    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        private IQueryable<User> FindByCustomerId(int customerId)
        {
            return this.DataContext.Users.Where(u => u.Customer_Id == customerId);
        }

        public bool UserHasActiveCase(int customerId, int userId, List<int> workingGroupIds)
        {
            if (workingGroupIds.Any())
            {
                var allUserCases =
                    this.DataContext.Cases.Where(
                        c => c.Customer_Id == customerId && c.Performer_User_Id == userId && c.WorkingGroup_Id != null && c.FinishingDate == null)
                        .Select(c => new { c.WorkingGroup_Id })
                        .ToList();

                var validCases = allUserCases.Where(a => workingGroupIds.Contains(a.WorkingGroup_Id.Value)).ToList();
                var invalidCases = allUserCases.Except(validCases).ToList();

                if (invalidCases.Any())
                    return true;
                else
                    return false;
            }
            else
            {
                var allUserCases =
                    this.DataContext.Cases.Where(
                        c => c.Customer_Id == customerId && c.Performer_User_Id == userId && c.FinishingDate == null)
                        .Select(c => new { c.Id })
                        .ToList();

                if (allUserCases.Any())
                    return true;
                else
                    return false;
            }

        }

        public int? GetUserDefaultWorkingGroupId(int userId, int customerId)
        {
            var entities = (from cu in this.DataContext.CustomerUsers.Where(x => x.User_Id == userId)
                            join c in this.DataContext.Customers.Where(c => c.Id == customerId) on cu.Customer_Id equals c.Id
                            join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                            join u in this.DataContext.Users on userId equals u.Id
                            from uwg in this.DataContext.UserWorkingGroups.Where(x => x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
                            where uwg.IsDefault == 1
                            select wg.Id).ToList();

            if (entities.Any())
            {
                return entities.First();
            }

            return null;
        }

        public string GetUserTimeZoneId(int userId)
        {
            var entity = (from u in this.DataContext.Users.Where(u => u.Id == userId)                                                        
                          select u.TimeZoneId).FirstOrDefault();

            return entity == null ? string.Empty : entity;            
        }

        public WorkingGroupEntity GetUserDefaultWorkingGroup(int userId, int customerId)
        {
            var entities = (from cu in this.DataContext.CustomerUsers.Where(x => x.User_Id == userId)
                            join c in this.DataContext.Customers.Where(c => c.Id == customerId) on cu.Customer_Id equals c.Id
                            join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                            join u in this.DataContext.Users on userId equals u.Id
                            from uwg in this.DataContext.UserWorkingGroups.Where(x => x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
                            where uwg.IsDefault == 1
                            select wg).ToList();

            if (entities.Any())
            {
                return entities.First();
            }

            return null;
        }

        public List<ItemOverview> FindActiveUsersIncludeEmails(int customerId)
        {
            var activeUsersWithIncludedEmails =
                this.DataContext.Users.Where(
                    u => u.Customer_Id == customerId && u.IsActive != 0 && u.Email != string.Empty)
                    .Select(u => new { u.FirstName, u.SurName, u.Email })
                    .ToList();

            return
                activeUsersWithIncludedEmails.Select(
                    u => new ItemOverview(u.FirstName + " " + u.SurName, u.Email.Split(";").First())).ToList();
        }

        public UserName GetUserNameById(int userId)
        {
            var user =
                this.DataContext.Users.Where(u => u.Id == userId).Select(u => new { u.FirstName, u.SurName }).Single();

            return new UserName(user.FirstName, user.SurName);
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            var users = this.FindByCustomerId(customerId).Where(u => u.IsActive != 0);
            var overviews = users.Select(u => new { Name = u.FirstName + u.SurName, Value = u.Id }).ToList();

            return
                overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(o => o.Name)
                    .ToList();
        }

        public List<ItemOverview> FindUsersWithPermissionsForCustomers(int[] customers)
        {
            bool isFirstName = true;
            if (customers.Any())
            {
                var customerId = customers[0];
                var cs = this.DataContext.Settings.Where(s => s.Customer_Id == customerId).SingleOrDefault();
                if (cs != null)
                    isFirstName = (cs.IsUserFirstLastNameRepresentation == 1);
            }

            var users =
                this.DataContext.Users.Where(
                        u => u.IsActive != 0)
                        .Select(u => new { Name = (isFirstName ? u.FirstName + " " + u.SurName : u.SurName + " " + u.FirstName), Value = u.Id, u.Cs })
                    .ToList();

            return
                users
                    .Where(u => u.Cs != null &&
                        u.Cs.Select(c => c.Id).Intersect(customers).Any())
                    .Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(o => o.Name)
                    .ToList();
        }

        public List<ItemWithEmail> FindUsersEmails(List<int> userIds, bool activeOnly = false)
        {
            var query = Table.Where(u => userIds.Contains(u.Id) && u.Email.Length > 1);

            if (activeOnly)
                query = query.Where(u => u.IsActive == 1);

           var usersEmails = query.Select(u => new {u.Id, u.Email}).ToList();
           return usersEmails.Select(u => new ItemWithEmail(u.Id, u.Email)).ToList();
        }

        public User GetUserForCopy(int id)
        {
            var user = Table.AsNoTracking().Include(x => x.CustomerUsers).FirstOrDefault(x => x.Id == id);
            DataContext.Entry(user).State = EntityState.Detached;
            return user;
        }

        public CustomerUserInfo GetUserInfo(int userId)
        {
            var userInfo = 
                Table.Where(u => u.Id == userId)
                .Select(x => new CustomerUserInfo()
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Performer = x.Performer,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    SurName = x.SurName
                })
                .FirstOrDefault();

            return userInfo;
        }

        public IQueryable<User> GetUsersForWorkingGroupQuery(int workingGroupId, int? customerId, bool requireMemberOfGroup = false)
        {
            bool checkGroup = requireMemberOfGroup;
            var query = 
                from u in this.DataContext.Users
                join uw in this.DataContext.UserWorkingGroups on u.Id equals uw.User_Id
                where uw.WorkingGroup_Id == workingGroupId  &&
                      (!checkGroup || (uw.UserRole != WorkingGroupUserPermission.READ_ONLY && uw.IsMemberOfGroup))
                select u;

            if (customerId > 0)
            {
                query = query.Where(u => u.CustomerUsers.Any(c => c.Customer_Id == customerId.Value));
            }

            return query.OrderBy(u => u.SurName).ThenBy(u => u.SurName);
        }

        public IQueryable<User> GetUsers(int customerId)
        {
            var query = from u in this.DataContext.Users
                        where u.CustomerUsers.Any(c => c.Customer_Id == customerId) // u.Customer_Id == customerId &&
                        select u;

            return query;
        }

        public IQueryable<User> GetUsersByUserGroup(int customerId)
        {
            var query = from u in this.DataContext.Users
                        where u.CustomerUsers.Any(c => c.Customer_Id == customerId && c.User.UserGroup_Id != 4) // u.Customer_Id == customerId &&
                        select u;

            return query;
        }

        public IQueryable<User> FindUsersByUserId(string userId)
        {
            var userIdUpper = (userId ?? string.Empty).ToUpper();
            return DataContext.Users.Where(u => u.UserID.ToUpper() == userIdUpper);
        }

        public IEnumerable<User> FindUsersByName(string name)
        {
            return this.DataContext.Users
                        .Where(u => u.FirstName.ToLower().Contains(name.ToLower()) || u.SurName.ToLower().Contains(name.ToLower()))
                        .ToList();
        }

        public IList<CustomerWorkingGroupForUser> GetWorkinggroupsForUserAndCustomer(int userId, int customerId)
        {
            var query = from uwg in this.DataContext.UserWorkingGroups.Where(uw => uw.User_Id == userId)
                        join wg in this.DataContext.WorkingGroups.Where(w => w.Customer_Id == customerId) on uwg.WorkingGroup_Id equals wg.Id
                        select new CustomerWorkingGroupForUser
                        {
                            WorkingGroupName = wg.WorkingGroupName,
                            User_Id = uwg.User_Id,
                            WorkingGroup_Id = uwg.WorkingGroup_Id,
                            RoleToUWG = uwg.UserRole,
                            IsMemberOfGroup = uwg.IsMemberOfGroup,
                            IsActive = wg.IsActive != 0
                        };

            var queryList = query.OrderBy(x => x.WorkingGroupName).ToList();
            return queryList;
        }

        public IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId)
        {
            var query = from cu in this.DataContext.CustomerUsers.Where(x => x.User_Id == userId)
                        join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                        join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                        from uwg in this.DataContext.UserWorkingGroups.Where(x => x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
                        group uwg by new { wg.WorkingGroupName, userId, c.Name, wg.Id, uwg.UserRole, uwg.IsMemberOfGroup, CustomerId = c.Id, uwg.IsDefault, wg.IsActive } into g
                        select new CustomerWorkingGroupForUser
                        {
                            WorkingGroupName = g.Key.WorkingGroupName,
                            User_Id = userId,
                            CustomerName = g.Key.Name,
                            IsStandard = g.Key.IsDefault,
                            WorkingGroup_Id = g.Key.Id,
                            RoleToUWG = (int?)g.Key.UserRole ?? WorkingGroupUserPermission.NO_ACCESS,
                            IsMemberOfGroup = (bool?)g.Key.IsMemberOfGroup ?? false,
                            IsActive = g.Key.IsActive != 0,
                            CustomerId = g.Key.CustomerId
                        };

            var queryList = query.OrderBy(x => x.CustomerName + x.WorkingGroupName).ToList();
            return queryList;
        }

        public IList<User> GetUsersForUserSettingList(UserSearch searchUser)
        {

            var query = from u in this.DataContext.Users
                        join cu in this.DataContext.CustomerUsers on u.Id equals cu.User_Id into cuGroup
                        from cuOJ in cuGroup.DefaultIfEmpty()
                        where ((cuOJ != null && cuOJ.Customer_Id == searchUser.CustomerId))
                        select u;


            if (searchUser.StatusId == 2)
                query = query.Where(x => x.IsActive == 0);
            else if (searchUser.StatusId == 1)
                query = query.Where(x => x.IsActive == 1); //&& x.Customer_Id == searchUser.CustomerId
            else if (searchUser.StatusId == 3)
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

            return query.OrderBy(x => x.SurName).ThenBy(x => x.FirstName).Distinct().ToList();
            //return query.OrderBy(x => x.UserID).Distinct().ToList();
        }

        public IList<User> GetUsersForUserSettingListByUserGroup(UserSearch searchUser)
        {

            var query = from u in this.DataContext.Users
                        join cu in this.DataContext.CustomerUsers on u.Id equals cu.User_Id into cuGroup
                        from cuOJ in cuGroup.DefaultIfEmpty()
                        where ((cuOJ != null && cuOJ.Customer_Id == searchUser.CustomerId) && cuOJ.User.UserGroup_Id != 4)
                        select u;


            if (searchUser.StatusId == 2)
                query = query.Where(x => x.IsActive == 0);
            else if (searchUser.StatusId == 1)
                query = query.Where(x => x.IsActive == 1 /*&& x.Customer_Id == searchUser.CustomerId*/);
            else if (searchUser.StatusId == 3)
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

            return query.OrderBy(x => x.SurName).ThenBy(x => x.FirstName).Distinct().ToList();
            //return query.OrderBy(x => x.UserID).Distinct().ToList();
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

        public DateTime GetPasswordChangedDate(int id)
        {
            return DataContext.Users.Where(x => x.Id == id).Select(x => x.PasswordChangedDate).FirstOrDefault();
        }

        public UserLoginInfo GetUserLoginInfo(string userId)
        {
            var userIdUpper = (userId ?? string.Empty).ToUpper().Trim();

            var user = 
                Table.Where(u => u.UserID.ToUpper() == userIdUpper && u.IsActive == 1)
                .Select(u => new UserLoginInfo()
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserId = userId,
                    Password = u.Password,
                    UserGroupId = u.UserGroup_Id
                }).FirstOrDefault();

            return user;
        }
        
        public UserOverview GetUser(int userId)
        {
            var user = this.GetUser(x => x.Id == userId);
            return user;
        }

        public async Task<UserOverview> GetUserAsync(int userId)
        {
            return await GetUserAsync(x => x.Id == userId);
        }
        
        public IList<UserLists> GetUserOnCases(int customerId, bool isTakeOnlyActive = false)
        {
            var query = from u in this.DataContext.Users
                        join ca in this.DataContext.Cases on u.Id equals ca.User_Id
                        where (ca.Customer_Id == customerId && (!isTakeOnlyActive || (isTakeOnlyActive && u.IsActive == 1)))
                        group u by new { u.Id, u.FirstName, u.SurName } into g
                        select new UserLists { Id = g.Key.Id, FirstName = g.Key.FirstName, LastName = g.Key.SurName };

            return query.ToList();
        }

        public UserOverview GetUserByLogin(string userId, int? customerId = null)
        {
            var userIdUpper = (userId ?? string.Empty).ToUpper().Trim();
            Expression<Func<User, bool>> expression = x => x.UserID.ToUpper() == userIdUpper && x.IsActive == 1;

            if (customerId.HasValue)
                expression = PredicateBuilder<User>.AndAlso(expression, x => x.Customer_Id == customerId.Value);

            var user = this.GetUser(expression);
            return user;
        }

        public UserName GetUserName(int userId)
        {
            return this.DataContext.Users.Where(u => u.Id == userId)
                    .Select(u => new UserName(u.FirstName, u.SurName))
                    .FirstOrDefault();
        }

        public ItemOverview FindActiveOverview(int userId)
        {
            var users = this.DataContext.Users
                        .Where(u => u.Id == userId && u.IsActive != 0)
                        .Select(u => new { Name = u.FirstName + u.SurName, Value = u.Id })
                        .ToList();

            return users.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).FirstOrDefault();
        }
        
        public async Task<UserOverview> GetByUserIdAsync(string userId, string passw)
        {
            var userIdUpper = (userId ?? string.Empty).ToUpper().Trim();
            var selector = GetUserOverviewSelector();

            var res = await DataContext.Users.Where(u => u.UserID.ToUpper() == userIdUpper && u.Password == passw && u.IsActive == 1).Select(selector).ToListAsync();
            return res.FirstOrDefault();
        }
        
        private UserOverview GetUser(Expression<Func<User, bool>> expression)
        {
            return GetUserQuery(expression).SingleOrDefault();
        }

        private async Task<UserOverview> GetUserAsync(Expression<Func<User, bool>> expression)
        {
            return await GetUserQuery(expression).SingleOrDefaultAsync();
        }

        private IQueryable<UserOverview> GetUserQuery(Expression<Func<User, bool>> expression)
        {
            var selector = GetUserOverviewSelector();
            return Table.Where(expression).Select(selector);
        }

        private Expression<Func<User, UserOverview>> GetUserOverviewSelector()
        {
            Expression<Func<User, UserOverview>> exp =
                x => new UserOverview()
                {
                    Id = x.Id,
                    UserId = x.UserID,
                    CustomerId = x.Customer_Id,
                    LanguageId = x.Language_Id,
                    UserGroupId = x.UserGroup_Id,
                    FollowUpPermission = x.FollowUpPermission,
                    RestrictedCasePermission = x.RestrictedCasePermission,
                    ShowNotAssignedWorkingGroups = x.ShowNotAssignedWorkingGroups,
                    CreateCasePermission = x.CreateCasePermission,
                    CreateSubCasePermission = x.CreateSubCasePermission,
                    CopyCasePermission = x.CopyCasePermission,
                    OrderPermission = x.OrderPermission,
                    CaseSolutionPermission = x.CaseSolutionPermission,
                    DeleteCasePermission = x.DeleteCasePermission,
                    DeleteAttachedFilePermission = x.DeleteAttachedFilePermission,
                    MoveCasePermission = x.MoveCasePermission,
                    ActivateCasePermission = x.ActivateCasePermission,
                    ReportPermission = x.ReportPermission,
                    CloseCasePermission = x.CloseCasePermission,
                    CalendarPermission = x.CalendarPermission,
                    FAQPermission = x.FAQPermission,
                    BulletinBoardPermission = x.BulletinBoardPermission,
                    DocumentPermission = x.DocumentPermission,
                    InventoryAdminPermission = x.InventoryPermission,
					InventoryViewPermission = x.InventoryViewPermission,
                    ContractPermission = x.ContractPermission,
                    SetPriorityPermission = x.SetPriorityPermission,
                    InvoicePermission = x.InvoicePermission,
                    DataSecurityPermission = x.DataSecurityPermission,
                    CaseUnlockPermission = x.CaseUnlockPermission,
                    RefreshContent = x.RefreshContent,
                    FirstName = x.FirstName,
                    SurName = x.SurName,
                    Phone = x.Phone,
                    Email = x.Email,
                    UserWorkingGroups = x.UserWorkingGroups,
                    StartPage = x.StartPage,
                    ShowSolutionTime = x.ShowSolutionTime != 0,
                    ShowCaseStatistics = x.ShowCaseStatistics != 0,
                    TimeZoneId = x.TimeZoneId,
                    UserGUID = x.UserGUID,
                    CaseInternalLogPermission = x.CaseInternalLogPermission,
                    InvoiceTimePermission = x.InvoiceTimePermission,
                    DefaultWorkingGroupId = x.Default_WorkingGroup_Id
                };

            return exp;
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

    #endregion
}

