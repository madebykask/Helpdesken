namespace DH.Helpdesk.Services.BusinessLogic.Admin.Users
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.Domain;

    public interface IUserPermissionsChecker
    {
        bool UserHasPermission(User user, UserPermission permission);

        bool UserHasAllPermissions(User user, List<UserPermission> permissions);

        bool UserHasAnyPermissions(User user, List<UserPermission> permissions);

        List<UserPermission> GetUserPermissions(User user); 

        bool CheckPermissions(User user, out List<UserPermission> wrongPermissions);
    }
}