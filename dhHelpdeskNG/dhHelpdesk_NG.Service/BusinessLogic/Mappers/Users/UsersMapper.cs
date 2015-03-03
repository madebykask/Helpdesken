namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.User.Input;
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
                RestrictedCasePermission = overview.RestrictedCasePermission,
                ShowNotAssignedWorkingGroups = overview.ShowNotAssignedWorkingGroups,
                CreateCasePermission = overview.CreateCasePermission,
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
                StartPage = overview.StartPage
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
                user.RestrictedCasePermission,
                user.ShowNotAssignedWorkingGroups,
                user.CreateCasePermission,
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
                user.SetPriorityPermission,
                user.InvoicePermission,
                user.DataSecurityPermission,
                user.RefreshContent,
                user.FirstName,
                user.SurName,
                user.Phone,
                user.Email,
                user.UserWorkingGroups,
                user.StartPage);
        }

        public static List<CustomerSettings> MapToUserCustomersSettings(
                                    IQueryable<Customer> customers,
                                    IQueryable<User> users,
                                    IQueryable<CustomerUser> customerUsers,
                                    IQueryable<Setting> customerSettings,
                                    IEntityToBusinessModelMapper<Setting, CustomerSettings> mapper)
        {
            var entities = (from cu in customerUsers
                            join c in customers on cu.Customer_Id equals c.Id
                            join u in users on cu.User_Id equals u.Id
                            join cs in customerSettings on cu.Customer_Id equals cs.Customer_Id
                            select cs)
                            .ToList();

            return entities.Select(mapper.Map).ToList();
        }
    }
}