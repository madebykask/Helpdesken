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

            if (user.CreateCasePermission.ToBool())
            {
                permissions.Add(UserPermission.CreateCasePermission);
            }

            if (user.CreateSubCasePermission.ToBool())
            {
                permissions.Add(UserPermission.CreateSubCasePermission);
            }

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

            if (user.CloseCasePermission.ToBool())
            {
                permissions.Add(UserPermission.CloseCasePermission);
            }

            if (user.RestrictedCasePermission.ToBool())
            {
                permissions.Add(UserPermission.RestrictedCasePermission);
            }

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

            if (user.BulletinBoardPermission.ToBool())
            {
                permissions.Add(UserPermission.BulletinBoardPermission);
            }

            if (user.DocumentPermission.ToBool())
            {
                permissions.Add(UserPermission.DocumentPermission);
            }

            if (user.InventoryPermission.ToBool())
            {
                permissions.Add(UserPermission.InventoryPermission);
            }

            if (user.ContractPermission)
            {
                permissions.Add(UserPermission.ContractPermission);
            }

            if (user.InvoicePermission.ToBool())
            {
                permissions.Add(UserPermission.InvoicePermission);
            }

            if (user.CaseUnlockPermission.ToBool())
            {
                permissions.Add(UserPermission.CaseUnlockPermission);
            }

            if (user.CaseInternalLogPermission.ToBool())
            {
                permissions.Add(UserPermission.CaseInternalLogPermission);
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

                    if (permissions.Contains(UserPermission.Performer))
                    {
                        wrongPermissions.Add(UserPermission.Performer);
                    }

                    if (permissions.Contains(UserPermission.CopyCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.CopyCasePermission);
                    }

                    if (permissions.Contains(UserPermission.DeleteCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.DeleteCasePermission);
                    }

                    if (permissions.Contains(UserPermission.DeleteAttachedFilePermission))
                    {
                        wrongPermissions.Add(UserPermission.DeleteAttachedFilePermission);
                    }

                    if (permissions.Contains(UserPermission.MoveCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.MoveCasePermission);
                    }

                    if (permissions.Contains(UserPermission.ActivateCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.ActivateCasePermission);
                    }

                    if (permissions.Contains(UserPermission.FollowUpPermission))
                    {
                        wrongPermissions.Add(UserPermission.FollowUpPermission);
                    }

                    if (permissions.Contains(UserPermission.DataSecurityPermission))
                    {
                        wrongPermissions.Add(UserPermission.DataSecurityPermission);
                    }

                    if (permissions.Contains(UserPermission.CaseSolutionPermission))
                    {
                        wrongPermissions.Add(UserPermission.CaseSolutionPermission);
                    }

                    if (permissions.Contains(UserPermission.ReportPermission))
                    {
                        wrongPermissions.Add(UserPermission.ReportPermission);
                    }

                    if (permissions.Contains(UserPermission.FaqPermission))
                    {
                        wrongPermissions.Add(UserPermission.FaqPermission);
                    }

                    if (permissions.Contains(UserPermission.CalendarPermission))
                    {
                        wrongPermissions.Add(UserPermission.CalendarPermission);
                    }

                    if (permissions.Contains(UserPermission.CreateOrderPermission))
                    {
                        wrongPermissions.Add(UserPermission.CreateOrderPermission);
                    }

                    if (permissions.Contains(UserPermission.AdministerOrderPermission))
                    {
                        wrongPermissions.Add(UserPermission.AdministerOrderPermission);
                    }

                    break;
                case UserGroup.Administrator:
                    // add permissions for Handläggare here
                    break;
                case UserGroup.CustomerAdministrator:
                    if (permissions.Contains(UserPermission.RestrictedCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.RestrictedCasePermission);
                    }

                    break;
                case UserGroup.SystemAdministrator:
                    if (permissions.Contains(UserPermission.RestrictedCasePermission))
                    {
                        wrongPermissions.Add(UserPermission.RestrictedCasePermission);
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