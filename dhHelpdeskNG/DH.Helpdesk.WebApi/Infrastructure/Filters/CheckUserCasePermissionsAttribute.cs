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
}