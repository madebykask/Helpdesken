﻿using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Users;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region USER

    public interface IUserRepository : IRepository<User>
    {
        List<ItemOverview> FindActiveUsersIncludeEmails(int customerId);
        
        UserName GetUserNameById(int userId);

        List<ItemOverview> FindActiveOverviews(int customerId);

        List<ItemWithEmail> FindUsersEmails(List<int> userIds);

        IEnumerable<User> GetUsers(int customerId);
        IEnumerable<User> GetUsersForWorkingGroup(int customerId, int workingGroupId);
        IEnumerable<User> GetUsersForWorkingGroup(int workingGroupId);
        IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId);
        IList<CustomerWorkingGroupForUser> GetWorkinggroupsForUserAndCustomer(int userId, int customerId);
        IList<LoggedOnUsersOnIndexPage> LoggedOnUsers();
        IList<UserLists> GetUserOnCases(int customer);
        IList<User> GetUsersForUserSettingList(UserSearch searchUser);
        UserOverview Login(string uId, string pwd);
        UserOverview GetUser(int userid);
        UserOverview GetUserByLogin(string IdName);

        UserName GetUserName(int userId);

        ItemOverview FindActiveOverview(int userId);

        List<ItemOverview> FindUsersWithPermissionsForCustomers(int[] customers);

        IEnumerable<User> FindUsersByName(string name);

        bool UserHasCase(int customerId, int userId, List<int> workingGroupIds);
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

        public bool UserHasCase(int customerId, int userId, List<int> workingGroupIds)
        {            
            if (workingGroupIds.Any())
            {
                var allUserCases =
                    this.DataContext.Cases.Where(
                        c => c.Customer_Id == customerId && c.Performer_User_Id == userId && c.WorkingGroup_Id != null)
                        .Select(c => new { c.WorkingGroup_Id })
                        .ToList();

                var validCases =   allUserCases.Where(a => workingGroupIds.Contains(a.WorkingGroup_Id.Value)).ToList();
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
                        c => c.Customer_Id == customerId && c.Performer_User_Id == userId)
                        .Select(c => new { c.Id })
                        .ToList();

                if (allUserCases.Any())
                    return true;
                else
                    return false;
            }
                        
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
            var users =
                this.DataContext.Users.Where(
                        u => u.IsActive != 0)
                    .Select(u => new { Name = u.FirstName + u.SurName, Value = u.Id, u.Cs })
                    .ToList();

            return
                users
                    .Where(u => u.Cs != null &&
                        u.Cs.Select(c => c.Id).Intersect(customers).Any())
                    .Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(o => o.Name)
                    .ToList();
        }

        public List<ItemWithEmail> FindUsersEmails(List<int> userIds)
        {
            var usersEmails =
                this.DataContext.Users.Where(u => userIds.Contains(u.Id) && u.Email.Length > 1)
                    .Select(u => new { Id = u.Id, Email = u.Email })
                    .ToList();

            return usersEmails.Select(e => new ItemWithEmail(e.Id, e.Email)).ToList();
        }

        public IEnumerable<User> GetUsersForWorkingGroup(int customerId, int workingGroupId)
        {
            var query = from u in this.DataContext.Users
                        join uw in this.DataContext.UserWorkingGroups on u.Id equals uw.User_Id
                        where u.CustomerUsers.Any(c => c.Customer_Id == customerId) && uw.WorkingGroup_Id == workingGroupId //u.Customer_Id == customerId
                        select u;
            return query;
        }

        public IEnumerable<User> GetUsersForWorkingGroup(int workingGroupId)
        {
            var query = from u in this.DataContext.Users
                        join uw in this.DataContext.UserWorkingGroups on u.Id equals uw.User_Id
                        where uw.WorkingGroup_Id == workingGroupId
                        select u;
            return query;
        }

        public IEnumerable<User> GetUsers(int customerId)
        {
            var query = from u in this.DataContext.Users
                        where u.CustomerUsers.Any(c => c.Customer_Id == customerId) // u.Customer_Id == customerId &&
                        select u;

            return query;
        }

        public IEnumerable<User> FindUsersByName(string name)
        {
            return this.DataContext.Users
                        .Where(u => u.FirstName.ToLower().Contains(name.ToLower())
                                || u.SurName.ToLower().Contains(name.ToLower()))
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
                            RoleToUWG = uwg.UserRole 
                        };

            var queryList = query.OrderBy(x => x.WorkingGroupName).ToList();
            return queryList;
        }

        public IList<CustomerWorkingGroupForUser> ListForWorkingGroupsInUser(int userId)
        {
            var query = from cu in this.DataContext.CustomerUsers.Where(x => x.User_Id == userId)
                        join c in this.DataContext.Customers on cu.Customer_Id equals c.Id
                        join wg in this.DataContext.WorkingGroups on c.Id equals wg.Customer_Id
                        join u in this.DataContext.Users on userId equals u.Id
                        from uwg in this.DataContext.UserWorkingGroups.Where(x => x.WorkingGroup_Id == wg.Id && x.User_Id == userId).DefaultIfEmpty()
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

        public IList<User> GetUsersForUserSettingList(UserSearch searchUser)
        {
            var query = from u in this.DataContext.Users
                        join cu in this.DataContext.CustomerUsers on u.Id equals cu.User_Id into cuGroup
                        from cuOJ in cuGroup.DefaultIfEmpty()
                        where ((cuOJ != null && cuOJ.Customer_Id == searchUser.CustomerId) || u.Customer_Id == searchUser.CustomerId)
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

        public UserOverview Login(string uId, string pwd)
        {
            var user = this.GetUser(x => x.UserID == uId && x.IsActive == 1 && x.Password == pwd);
            return user;
        }

        public UserOverview GetUser(int userid)
        {
            var user = this.GetUser(x => x.Id == userid);
            return user;
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

        public UserOverview GetUserByLogin(string IdName)
        {
            var user = this.GetUser(x => x.UserID == IdName && x.IsActive == 1);
            return user;
        }

        public UserName GetUserName(int userId)
        {
            return
                this.DataContext.Users.Where(u => u.Id == userId)
                    .Select(u => new { u.FirstName, u.SurName })
                    .ToList()
                    .Select(u => new UserName(u.FirstName, u.SurName))
                    .FirstOrDefault();
        }

        public ItemOverview FindActiveOverview(int userId)
        {
            var users = this.DataContext.Users
                        .Where(u => u.Id == userId &&
                            u.IsActive != 0)
                        .Select(u => new { Name = u.FirstName + u.SurName, Value = u.Id })
                        .ToList();

            return users.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture)))
                        .FirstOrDefault();            
        }

        private UserOverview GetUser(Expression<Func<User, bool>> expression)
        {
            var u = this.DataContext.Users
                    .Where(expression)
                    .ToList() 
                    .Select(x => new UserOverview( 
                        x.Id,
                        x.UserID,
                        x.Customer_Id,
                        x.Language_Id, 
                        x.UserGroup_Id,
                        x.FollowUpPermission,
                        x.RestrictedCasePermission,
                        x.ShowNotAssignedWorkingGroups,
                        x.CreateCasePermission,
                        x.CopyCasePermission,
                        x.OrderPermission,
                        x.CaseSolutionPermission, 
                        x.DeleteCasePermission, 
                        x.DeleteAttachedFilePermission, 
                        x.MoveCasePermission ,
                        x.ActivateCasePermission,  
                        x.ReportPermission, 
                        x.CloseCasePermission,
                        x.CalendarPermission, 
                        x.FAQPermission, 
                        x.BulletinBoardPermission,
                        x.SetPriorityPermission, 
                        x.InvoicePermission, 
                        x.DataSecurityPermission, 
                        x.RefreshContent,
                        x.FirstName,
                        x.SurName,
                        x.Phone,
                        x.Email,
                        x.UserWorkingGroups,
                        x.StartPage)).SingleOrDefault();
            return u;
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

