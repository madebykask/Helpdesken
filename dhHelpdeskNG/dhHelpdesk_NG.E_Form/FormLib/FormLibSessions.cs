using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.FormLib
{
    public static class FormLibSessions
    {
        private const string _CURRENT_USER = "CURRENT_FORM_LIB_USER";
        private const string _CURRENT_CUSTOMER_ID = "CURRENT_CUSTOMER_ID";
        private const string _CURRENT_LANGUAGE = "CURRENT_LANGUAGE";
        private const string _CURRENT_SYSTEM_USER = "CURRENT_SYSTEM_USER";
        private const string _CURRENT_USER_IDENTITY = "CURRENT_USER_IDENTITY";
        private const string _USER_HAS_ACCESS = "USER_HAS_ACCESS";
        private const string _CURRENT_COWORKERS = "CURRENT_COWORKERS";

        public static int CustomerId
        {
            get
            {
                return (int)HttpContext.Current.Session[_CURRENT_CUSTOMER_ID];
            }
            set
            {
                if(HttpContext.Current.Session[_CURRENT_CUSTOMER_ID] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CUSTOMER_ID, value);
                else
                    HttpContext.Current.Session[_CURRENT_CUSTOMER_ID] = value;
            }
        }

        public static string CurrentSystemUser
        {
            get
            {
                if(HttpContext.Current.Session[_CURRENT_SYSTEM_USER] == null)
                    return null;
                return (string)HttpContext.Current.Session[_CURRENT_SYSTEM_USER];
            }
            set
            {
                if(HttpContext.Current.Session[_CURRENT_SYSTEM_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_SYSTEM_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_SYSTEM_USER] = value;
            }
        }

        public static UserIdentity CurrentUserIdentity
        {
            get
            {
                if(HttpContext.Current.Session[_CURRENT_USER_IDENTITY] == null)
                    return null;
                return (UserIdentity)HttpContext.Current.Session[_CURRENT_USER_IDENTITY];
            }
            set
            {
                if(HttpContext.Current.Session[_CURRENT_USER_IDENTITY] == null)
                    HttpContext.Current.Session.Add(_CURRENT_USER_IDENTITY, value);
                else
                    HttpContext.Current.Session[_CURRENT_USER_IDENTITY] = value;
            }
        }

        public static List<SubordinateResponseItem> CurrentCoWorkers
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_COWORKERS] == null)
                    return null;
                return (List<SubordinateResponseItem>)HttpContext.Current.Session[_CURRENT_COWORKERS];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_COWORKERS] == null)
                    HttpContext.Current.Session.Add(_CURRENT_COWORKERS, value);
                else
                    HttpContext.Current.Session[_CURRENT_COWORKERS] = value;
            }
        }

        public static bool UserHasAccess
        {
            get
            {
                if(HttpContext.Current.Session[_USER_HAS_ACCESS] == null)
                    return false;
                return (bool)HttpContext.Current.Session[_USER_HAS_ACCESS];
            }
            set
            {
                if(HttpContext.Current.Session[_USER_HAS_ACCESS] == null)
                    HttpContext.Current.Session.Add(_USER_HAS_ACCESS, false);
                else
                    HttpContext.Current.Session[_USER_HAS_ACCESS] = value;
            }
        }

        public static User User
        {
            get
            {
                return (User)HttpContext.Current.Session[_CURRENT_USER];
            }
            set
            {
                if(HttpContext.Current.Session[_CURRENT_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_USER] = value;
            }
        }
    }
}