namespace DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

    public sealed class UserPermissionsChecker : IUserPermissionsChecker
    {
        public bool UserHasPermission(User user, UserPermission permission)
        {
            return this.UserHasAllPermissions(user, new List<UserPermission> { permission });
        }

        public bool UserHasAllPermissions(User user, List<UserPermission> permissions)
        {
            var userPermissions = this.GetUserPermissions(user);
            return permissions.All(userPermissions.Contains);
        }

        public bool UserHasAnyPermissions(User user, List<UserPermission> permissions)
        {
            var userPermissions = this.GetUserPermissions(user);
            return permissions.Any(userPermissions.Contains);
        }

        public List<UserPermission> GetUserPermissions(User user)
        {
            var permissions = new List<UserPermission>();

            if (user.Performer.ToBool())
            {
                permissions.Add(UserPermission.Performer);
            }

            //Commented out because redmine 11954
            //if (user.CreateCasePermission.ToBool())
            //{
            //    permissions.Add(UserPermission.CreateCasePermission);
            //}

            if (user.CopyCasePermission.ToBool())
            {
                permissions.Add(UserPermission.CopyCasePermission);
            }

            if (user.DeleteCasePermission.ToBool())
            {
                permissions.Add(UserPermission.DeleteCasePermission);
            }

            if (user.DeleteAttachedFilePermission.ToBool())
            {
                permissions.Add(UserPermission.DeleteAttachedFilePermission);
            }

            if (user.MoveCasePermission.ToBool())
            {
                permissions.Add(UserPermission.MoveCasePermission);
            }

            if (user.ActivateCasePermission.ToBool())
            {
                permissions.Add(UserPermission.ActivateCasePermission);
            }

            //Commented out because redmine 11954
            //if (user.CloseCasePermission.ToBool())
            //{
            //    permissions.Add(UserPermission.CloseCasePermission);
            //}
            //Commented out because redmine 11954
            //if (user.RestrictedCasePermission.ToBool())
            //{
            //    permissions.Add(UserPermission.RestrictedCasePermission);
            //}

            if (user.FollowUpPermission.ToBool())
            {
                permissions.Add(UserPermission.FollowUpPermission);
            }

            if (user.DataSecurityPermission.ToBool())
            {
                permissions.Add(UserPermission.DataSecurityPermission);
            }

            if (user.CaseSolutionPermission.ToBool())
            {
                permissions.Add(UserPermission.CaseSolutionPermission);
            }

            if (user.ReportPermission.ToBool())
            {
                permissions.Add(UserPermission.ReportPermission);
            }

            if (user.FAQPermission.ToBool())
            {
                permissions.Add(UserPermission.FaqPermission);
            }

            if (user.CalendarPermission.ToBool())
            {
                permissions.Add(UserPermission.CalendarPermission);
            }

            if (user.OrderPermission == (int)UserOrderPermission.Create)
            {
                permissions.Add(UserPermission.CreateOrderPermission);
            }

            if (user.OrderPermission == (int)UserOrderPermission.Administer)
            {
                permissions.Add(UserPermission.AdministerOrderPermission);
            }

            return permissions;
        }

        public bool CheckPermissions(User user, out List<UserPermission> wrongPermissions)
        {
            wrongPermissions = new List<UserPermission>();
            var permissions = this.GetUserPermissions(user);

            switch ((UserGroup)user.UserGroup_Id)
            {
                case UserGroup.User:
                    //Commented out because redmine 11954
                    //if (!permissions.Contains(UserPermission.CreateCasePermission))
                    //{
                    //    wrongPermissions.Add(UserPermission.CreateCasePermission);
                    //}
                    //if (!permissions.Contains(UserPermission.RestrictedCasePermission))
                    //{
                    //    wrongPermissions.Add(UserPermission.RestrictedCasePermission);
                    //}

                    wrongPermissions.AddRange(permissions);

                    break;
                case UserGroup.Administrator:
                    break;
                case UserGroup.CustomerAdministrator:
                case UserGroup.SystemAdministrator:
                    if (!permissions.Contains(UserPermission.FaqPermission))
                    {
                        wrongPermissions.Add(UserPermission.FaqPermission);
                    }

                    break;
                default:
                    wrongPermissions.AddRange(permissions);
                    break;
            }

            return !wrongPermissions.Any();
        }
    }
}