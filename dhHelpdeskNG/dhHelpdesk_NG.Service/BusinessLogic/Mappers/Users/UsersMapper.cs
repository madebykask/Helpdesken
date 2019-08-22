namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Users
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.Models.Users;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

    public static class UsersMapper
    {
        public static List<User> MapToCustomerUsers(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> cusstomerUsers)
        {
            var entities = (from cu in cusstomerUsers
                            join c in customers on cu.Customer_Id equals c.Id
                            join u in users on cu.User_Id equals u.Id
                            select u)
                            .GetOrderedByName()
                            .ToList();

            return entities;
        }

        public static List<ItemOverview> MapToCustomerUsersOverviews(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> customerUsers)
        {
            var entities = (from cu in customerUsers
                            join c in customers on cu.Customer_Id equals c.Id
                            join u in users on cu.User_Id equals u.Id
                            select u)
                            .GetOrderedByName()
                            .ToList();

            return entities.Select(u => new ItemOverview(
                                        new UserName(u.FirstName, u.SurName).GetReversedFullName(),
                                        u.Id.ToString(CultureInfo.InvariantCulture)))
                                        .ToList();
        }

        public static User MapToUser(UserOverview overview)
        {
            return new User 
            {
                Id = overview.Id,
                UserID = overview.UserId,
                Customer_Id = overview.CustomerId,
                Language_Id = overview.LanguageId,
                UserGroup_Id = overview.UserGroupId,
                FollowUpPermission = overview.FollowUpPermission,
                //RestrictedCasePermission = overview.RestrictedCasePermission,
                ShowNotAssignedWorkingGroups = overview.ShowNotAssignedWorkingGroups,
                CreateCasePermission = overview.CreateCasePermission,
                CreateSubCasePermission = overview.CreateSubCasePermission,
                CopyCasePermission = overview.CopyCasePermission,
                OrderPermission = overview.OrderPermission,
                CaseSolutionPermission = overview.CaseSolutionPermission,
                DeleteCasePermission = overview.DeleteCasePermission,
                DeleteAttachedFilePermission = overview.DeleteAttachedFilePermission,
                MoveCasePermission = overview.MoveCasePermission,
                ActivateCasePermission = overview.ActivateCasePermission,
                ReportPermission = overview.ReportPermission,
                CloseCasePermission = overview.CloseCasePermission,
                CalendarPermission = overview.CalendarPermission,
                FAQPermission = overview.FAQPermission,
                BulletinBoardPermission = overview.BulletinBoardPermission,
                SetPriorityPermission = overview.SetPriorityPermission,
                InvoicePermission = overview.InvoicePermission,
                DataSecurityPermission = overview.DataSecurityPermission,
                RefreshContent = overview.RefreshContent,
                FirstName = overview.FirstName,
                SurName = overview.SurName,
                Phone = overview.Phone,
                Email = overview.Email,
                UserWorkingGroups = overview.UserWorkingGroups,
                StartPage = overview.StartPage,
                DocumentPermission = overview.DocumentPermission,
                InventoryPermission = overview.InventoryAdminPermission,
                InventoryViewPermission = overview.InventoryViewPermission,
                ContractPermission = overview.ContractPermission,
                CaseUnlockPermission = overview.CaseUnlockPermission,
                CaseInternalLogPermission = overview.CaseInternalLogPermission,
                InvoiceTimePermission = overview.InvoiceTimePermission
            };
        }

        public static UserOverview MapToOverview(User user)
        {
            return new UserOverview(
                user.Id,
                user.UserID,
                user.Customer_Id,
                user.Language_Id,
                user.UserGroup_Id,
                user.FollowUpPermission,
                //user.RestrictedCasePermission,
                user.ShowNotAssignedWorkingGroups,
                user.CreateCasePermission,
                user.CreateSubCasePermission,
                user.CopyCasePermission,
                user.OrderPermission,
                user.CaseSolutionPermission,
                user.DeleteCasePermission,
                user.DeleteAttachedFilePermission,
                user.MoveCasePermission,
                user.ActivateCasePermission,
                user.ReportPermission,
                user.CloseCasePermission,
                user.CalendarPermission,
                user.FAQPermission,
                user.BulletinBoardPermission,
                user.DocumentPermission,
                user.InventoryPermission,
                user.InventoryViewPermission,
                user.ContractPermission,
                user.SetPriorityPermission,
                user.InvoicePermission,
                user.DataSecurityPermission,
                user.CaseUnlockPermission,
                user.RefreshContent,
                user.FirstName,
                user.SurName,
                user.Phone,
                user.Email,
                user.UserWorkingGroups,
                user.StartPage,
                user.ShowSolutionTime.ToBool(),
                user.ShowCaseStatistics.ToBool(),
                user.TimeZoneId,
                user.UserGUID,
                user.CaseInternalLogPermission,
                user.InvoiceTimePermission
                );
        }

        public static List<CustomerSettings> MapToUserCustomersSettings(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> customerUsers,
                                    IQueryable<Setting> customerSettings,
                                    IEntityToBusinessModelMapper<Setting, CustomerSettings> mapper)
        {
            var query =
                from cu in customerUsers
                join c in customers on cu.Customer_Id equals c.Id
                join u in users on cu.User_Id equals u.Id
                join cs in customerSettings on cu.Customer_Id equals cs.Customer_Id
                select  cs;

            var entities = query.ToList();
            return entities.Select(mapper.Map).ToList();
        }

        public static List<UserProfileCustomerSettings> MapToUserProfileCustomersSettings(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> customerUsers,
                                    IQueryable<Setting> customerSettings)
        {
            var entities = (from cu in customerUsers
                            join c in customers on cu.Customer_Id equals c.Id
                            join u in users on cu.User_Id equals u.Id
                            join cs in customerSettings on cu.Customer_Id equals cs.Customer_Id
                            select new
                            {
                                cu.Customer_Id,
                                c.Name,
                                cu.ShowOnStartPage
                            })
                            .OrderBy(r => r.Name)
                            .ToList();

            return entities.Select(e => new UserProfileCustomerSettings(e.Customer_Id, e.Name, e.ShowOnStartPage.ToDefaultTrueBool())).ToList();
        }

        public static List<ItemOverview> MapToWorkingGroupUsers(
                                    IQueryable<User> users,
                                    IQueryable<WorkingGroupEntity> workingGroups,
                                    IQueryable<UserWorkingGroup> userWorkingGroups,
                                    IQueryable<Customer> customers,
                                    IQueryable<CustomerUser> customerUsers,
                                    int? workingGroupId)
        {
            if (workingGroupId.HasValue)
            {
                var entities = (from uwg in userWorkingGroups
                                join wg in workingGroups on uwg.WorkingGroup_Id equals wg.Id
                                join u in users on uwg.User_Id equals u.Id
                                join cu in customerUsers on u.Id equals cu.User_Id
                                join c in customers on cu.Customer_Id equals c.Id
                                select new
                                {
                                    u.Id,
                                    u.FirstName,
                                    u.SurName
                                })
                                .OrderBy(u => u.SurName)
                                .ThenBy(u => u.FirstName)
                                .ToList();

                return entities.Select(u => new ItemOverview(new UserName(u.FirstName, u.SurName).GetReversedFullName(), u.Id.ToString(CultureInfo.InvariantCulture))).ToList();                
            }

            var allCustomerUsers = (from u in users
                            join cu in customerUsers on u.Id equals cu.User_Id
                            join c in customers on cu.Customer_Id equals c.Id
                            select new
                            {
                                u.Id,
                                u.FirstName,
                                u.SurName
                            })
                .OrderBy(u => u.SurName)
                .ThenBy(u => u.FirstName)
                .ToList();

            return allCustomerUsers.Select(u => new ItemOverview(new UserName(u.FirstName, u.SurName).GetReversedFullName(), u.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public static List<Customer> MapToUserCustomers(
                                IQueryable<Customer> customers,
                                IQueryable<User> users,
                                IQueryable<CustomerUser> customerUsers)
        {
            var entities = (from u in users
                            join cu in customerUsers on u.Id equals cu.User_Id
                            join c in customers on cu.Customer_Id equals c.Id
                            select c)
                            .OrderBy(c => c.Name)
                            .ToList();

            return entities;
        } 
    }
}