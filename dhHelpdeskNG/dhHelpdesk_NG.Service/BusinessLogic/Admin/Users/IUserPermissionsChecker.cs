namespace DH.Helpdesk.Services.BusinessLogic.Admin.Users
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.Domain;

    public interface IUserPermissionsChecker
    {
        bool UserHasPermission(User user, UserPermission permission);

        bool UserHasPermissions(User user, List<UserPermission> permissions);

        List<UserPermission> GetUserPermissions(User user); 

        bool CheckPermissions(User user, out List<UserPermission> wrongPermissions);
    }
}