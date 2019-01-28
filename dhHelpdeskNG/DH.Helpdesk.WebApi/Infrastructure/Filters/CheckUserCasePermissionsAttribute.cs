using System;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{
    /// <summary>
    /// Attribute is processed by UserCasePermissionsFilter authorization filter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckUserCasePermissionsAttribute : Attribute
    {
        public CheckUserCasePermissionsAttribute()
        {
        }

        public CheckUserCasePermissionsAttribute(string paramName, bool checkBody = false)
        {
            CaseIdParamName = paramName;
            CheckBody = checkBody;
        }

        public string CaseIdParamName
        {
            get;
            set;
        }

        public bool CheckBody
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Used to check user permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckUserPermissionsAttribute : Attribute
    {
        public UserPermission[] UserPermissions { get; set; }

        public CheckUserPermissionsAttribute(UserPermission[] userPermission)
        {
            UserPermissions = userPermission;
        }

        public CheckUserPermissionsAttribute(UserPermission userPermission)
        {
            UserPermissions = new [] { userPermission };
        }
    }
}