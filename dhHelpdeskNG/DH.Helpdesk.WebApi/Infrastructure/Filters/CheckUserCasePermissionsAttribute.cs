using System;

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

        public CheckUserCasePermissionsAttribute(string paramName)
        {
            CaseIdParamName = paramName;
        }

        public string CaseIdParamName
        {
            get;
            set;
        }
    }
}