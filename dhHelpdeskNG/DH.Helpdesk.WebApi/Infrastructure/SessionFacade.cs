using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.User.Input;

namespace DH.Helpdesk.WebApi.Infrastructure
{
    /// <summary>
    /// Using session in web api can introduce perfomance issues, because session is locking for write(read?). Has to use it because a lot of legacy code requires it.
    /// </summary>
    public static class SessionFacade
    {
        private const string _CURRENT_USER = "CURRENT_USER";

        public static UserOverview CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                    return null;
                return (UserOverview)HttpContext.Current.Session[_CURRENT_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_USER] = value;
            }
        }
    }
}