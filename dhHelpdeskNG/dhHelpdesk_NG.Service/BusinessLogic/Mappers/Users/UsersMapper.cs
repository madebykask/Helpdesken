namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.User.Input;
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
    }
}