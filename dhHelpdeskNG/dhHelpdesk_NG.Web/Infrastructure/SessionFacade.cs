using System.Collections.Generic;
using System.Web;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs; 
using dhHelpdesk_NG.Web.Models;  

namespace dhHelpdesk_NG.Web.Infrastructure
{
    public static class SessionFacade
    {
        private const string _CURRENT_USER = "CURRENT_USER";
        private const string _CURRENT_CASE_SEARCH = "CURRENT_CASE_SEARCH";
        private const string _CASE_TRANSLATION = "CASE_TRANSLATION";
        private const string _COMPUTER_USER_SEARCH = "COMPUTER_USER_SEARCH";
        private const string _CURRENT_CUSTOMER = "CURRENT_CUSTOMER";
        private const string _CURRENT_LANGUAGE = "CURRENT_LANGUAGE";
        private const string _SIGNED_IN_USER = "SIGNED_IN_USER";
        private const string _TEXT_TRANSLATION = "TEXT_TRANSLATION";
        private const string _ACTIVE_TAB = "ACTIVE_TAB";

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                    return null;
                return (User)HttpContext.Current.Session[_CURRENT_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_USER] = value;
            }
        }

        public static CaseSearchModel CurrentCaseSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CASE_SEARCH] == null)
                    return null;
                return (CaseSearchModel)HttpContext.Current.Session[_CURRENT_CASE_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CASE_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CASE_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_CASE_SEARCH] = value;
            }
        }

        public static int CurrentLanguage
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_LANGUAGE] == null)
                    return 0;
                return (int)HttpContext.Current.Session[_CURRENT_LANGUAGE];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_LANGUAGE] == null)
                    HttpContext.Current.Session.Add(_CURRENT_LANGUAGE, value);
                else
                    HttpContext.Current.Session[_CURRENT_LANGUAGE] = value;
            }
        }

        public static IList<Text> TextTranslation
        {
            get
            {
                return (IList<Text>)HttpContext.Current.Session[_TEXT_TRANSLATION];
            }
            set
            {
                if (HttpContext.Current.Session[_TEXT_TRANSLATION] == null)
                    HttpContext.Current.Session.Add(_TEXT_TRANSLATION, value);
                else
                    HttpContext.Current.Session[_TEXT_TRANSLATION] = value;
            }
        }

        public static IList<CaseFieldSettingsForTranslation> CaseTranslation
        {
            get
            {
                return (IList<CaseFieldSettingsForTranslation>)HttpContext.Current.Session[_CASE_TRANSLATION];
            }
            set
            {
                if (HttpContext.Current.Session[_CASE_TRANSLATION] == null)
                    HttpContext.Current.Session.Add(_CASE_TRANSLATION, value);
                else
                    HttpContext.Current.Session[_CASE_TRANSLATION] = value;
            }
        }

        public static User SignedInUser //TODO: gör den här listan till en IList kanske? iom det bör komma ut info om alla inloggade användare, inte bara en.. 
        {
            get
            {
                return (User)HttpContext.Current.Session[_SIGNED_IN_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_SIGNED_IN_USER] == null)
                    HttpContext.Current.Session.Add(_SIGNED_IN_USER, value);
                else
                    HttpContext.Current.Session[_SIGNED_IN_USER] = value;
            }
        }

        public static ComputerUserSearch CurrentComputerUserSearch
        {
            get
            {
                return (ComputerUserSearch)HttpContext.Current.Session[_COMPUTER_USER_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_COMPUTER_USER_SEARCH] == null)
                    HttpContext.Current.Session.Add(_COMPUTER_USER_SEARCH, value);
                else
                    HttpContext.Current.Session[_COMPUTER_USER_SEARCH] = value;
            }
        }

        public static Customer CurrentCustomer
        {
            get
            {
                return (Customer)HttpContext.Current.Session[_CURRENT_CUSTOMER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CUSTOMER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CUSTOMER, value);
                else
                    HttpContext.Current.Session[_CURRENT_CUSTOMER] = value;
            }
        }

        public static string ActiveTab
        {
            get
            {
                return (string)HttpContext.Current.Session[_ACTIVE_TAB];
            }
            set
            {
                if (HttpContext.Current.Session[_ACTIVE_TAB] == null)
                    HttpContext.Current.Session.Add(_ACTIVE_TAB, value);
                else
                    HttpContext.Current.Session[_ACTIVE_TAB] = value;
            }
        }
    }
}