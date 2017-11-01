using DH.Helpdesk.SelfService.Infrastructure.Session;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.Models.Error;
    using BusinessData.Models.Employee;

    public static class SessionFacade
    {
        private const string _CURRENT_USER = "CURRENT_USER";
        private const string _CURRENT_LOCAL_USER = "CURRENT_LOCAL_USER";
        private const string _CURRENT_CASE_SEARCH = "CURRENT_CASE_SEARCH";
        private const string _CASE_TRANSLATION = "CASE_TRANSLATION";
        private const string _COMPUTER_USER_SEARCH = "COMPUTER_USER_SEARCH";
        private const string _CURRENT_CUSTOMER = "CURRENT_CUSTOMER";
        private const string _CURRENT_CUSTOMER_ID = "CURRENT_CUSTOMER_ID";
        private const string _CURRENT_LANGUAGE = "CURRENT_LANGUAGE";
        private const string _SIGNED_IN_USER = "SIGNED_IN_USER";
        private const string _TEXT_TRANSLATION = "TEXT_TRANSLATION";
        private const string _ACTIVE_TAB = "ACTIVE_TAB";
        private const string _CURRENT_SYSTEM_USER = "CURRENT_SYSTEM_USER";

        private const string _PAGE_FILTERS = "PAGE_FILTERS";
        private const string _CUSTOM_VALUES = "CUSTOM_VALUES";
        private const string _ACTIVE_TABS = "ACTIVE_TABS";
        private const string _CURRENT_CALENDER_SEARCH = "CURRENT_CALENDER_SEARCH";
        private const string _CURRENT_BULLETINBOARD_SEARCH = "CURRENT_BULLETINBOARD_SEARCH";
        private const string _CURRENT_CASESOLUTION_SEARCH = "CURRENT_CASESOLUTION_SEARCH";
        private const string _CURRENT_OPERATIONLOG_SEARCH = "CURRENT_OPERATIONLOG_SEARCH";        
        private const string _CURRENT_DOCUMENT_SEARCH = "CURRENT_DOCUMENT_SEARCH";
        private const string _CURRENT_USER_IDENTITY = "CURRENT_USER_IDENTITY";
        private const string _CURRENT_COWORKERS = "CURRENT_COWORKERS";
        private const string _USER_HAS_ACCESS = "USER_HAS_ACCESS";
        private const string _ALL_LANGUAGES = "ALL_LANGUAGES";
        private const string _LAST_CORRECT_URL = "LAST_CORRECT_URL";
        private const string _LAST_ERROR = "LAST_ERROR";

        public static int CurrentCustomerID
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CUSTOMER_ID] == null)
                    return -1;
                return (int) HttpContext.Current.Session[_CURRENT_CUSTOMER_ID];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CUSTOMER_ID] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CUSTOMER_ID, value);
                else
                    HttpContext.Current.Session[_CURRENT_CUSTOMER_ID] = value;
            }
        }

        public static string CurrentSystemUser
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_SYSTEM_USER] == null)
                    return null;
                return (string) HttpContext.Current.Session[_CURRENT_SYSTEM_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_SYSTEM_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_SYSTEM_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_SYSTEM_USER] = value;
            }
        }

        public static bool UserHasAccess
        {
            get
            {
                if (HttpContext.Current.Session[_USER_HAS_ACCESS] == null)
                    return false;
                return (bool)HttpContext.Current.Session[_USER_HAS_ACCESS];
            }
            set
            {
                if (HttpContext.Current.Session[_USER_HAS_ACCESS] == null)
                    HttpContext.Current.Session.Add(_USER_HAS_ACCESS, value);
                else
                    HttpContext.Current.Session[_USER_HAS_ACCESS] = value;
            }
        }

        public static string LastCorrectUrl
        {
            get
            {
                if (HttpContext.Current.Session[_LAST_CORRECT_URL] == null)
                    return null;
                return (string )HttpContext.Current.Session[_LAST_CORRECT_URL];
            }
            set
            {
                if (HttpContext.Current.Session[_LAST_CORRECT_URL] == null)
                    HttpContext.Current.Session.Add(_LAST_CORRECT_URL, value);
                else
                    HttpContext.Current.Session[_LAST_CORRECT_URL] = value;
            }
        }

        public static ErrorModel LastError
        {
            get
            {
                if (HttpContext.Current.Session[_LAST_ERROR] == null)
                    return null;
                return (ErrorModel)HttpContext.Current.Session[_LAST_ERROR];
            }
            set
            {
                if (HttpContext.Current.Session[_LAST_ERROR] == null)
                    HttpContext.Current.Session.Add(_LAST_ERROR, value);
                else
                    HttpContext.Current.Session[_LAST_ERROR] = value;
            }
        }

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

        public static List<LanguageOverview> AllLanguages
        {
            get
            {
                if (HttpContext.Current.Session[_ALL_LANGUAGES] == null)
                    return null;
                return (List<LanguageOverview>)HttpContext.Current.Session[_ALL_LANGUAGES];
            }
            set
            {
                if (HttpContext.Current.Session[_ALL_LANGUAGES] == null)
                    HttpContext.Current.Session.Add(_ALL_LANGUAGES, value);
                else
                    HttpContext.Current.Session[_ALL_LANGUAGES] = value;
            }
        }

        public static UserIdentity CurrentUserIdentity
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_USER_IDENTITY] == null)
                    return null;
                return (UserIdentity)HttpContext.Current.Session[_CURRENT_USER_IDENTITY];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_USER_IDENTITY] == null)
                    HttpContext.Current.Session.Add(_CURRENT_USER_IDENTITY, value);
                else
                    HttpContext.Current.Session[_CURRENT_USER_IDENTITY] = value;
            }
        }

        public static CalendarSearch CurrentCalenderSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] == null)
                    return null;
                return (CalendarSearch) HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CALENDER_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] = value;
            }
        }

        public static DocumentSearch CurrentDocumentSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] == null)
                    return null;
                return (DocumentSearch)HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_DOCUMENT_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] = value;
            }
        }

        public static BulletinBoardSearch CurrentBulletinBoardSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] == null)
                    return null;
                return (BulletinBoardSearch)HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_BULLETINBOARD_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] = value;
            }
        }

        public static CaseSolutionSearch CurrentCaseSolutionSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] == null)
                    return null;
                return (CaseSolutionSearch)HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_CASESOLUTION_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] = value;
            }
        }

        public static OperationLogSearch CurrentOperationLogSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] == null)
                    return null;
                return (OperationLogSearch)HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] == null)
                    HttpContext.Current.Session.Add(_CURRENT_OPERATIONLOG_SEARCH, value);
                else
                    HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] = value;
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

        public static int CurrentLanguageId
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

        //public static User SignedInUser //TODO: gör den här listan till en IList kanske? iom det bör komma ut info om alla inloggade användare, inte bara en.. 
        //{
        //    get
        //    {
        //        return (User)HttpContext.Current.Session[_SIGNED_IN_USER];
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Session[_SIGNED_IN_USER] == null)
        //            HttpContext.Current.Session.Add(_SIGNED_IN_USER, value);
        //        else
        //            HttpContext.Current.Session[_SIGNED_IN_USER] = value;
        //    }
        //}

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

        public static UserOverview CurrentLocalUser
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_LOCAL_USER] == null)
                    return null;
                return (UserOverview)HttpContext.Current.Session[_CURRENT_LOCAL_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_LOCAL_USER] == null)
                    HttpContext.Current.Session.Add(_CURRENT_LOCAL_USER, value);
                else
                    HttpContext.Current.Session[_CURRENT_LOCAL_USER] = value;
            }
        }

        public static TFilters FindPageFilters<TFilters>(string pageName) where TFilters : class
        {
            var pagesFilters = (List<PageFilters>)HttpContext.Current.Session[_PAGE_FILTERS];

            var pageFilters = pagesFilters?.SingleOrDefault(f => f.PageName == pageName);

            return (TFilters) pageFilters?.Filters;
        }

        public static void SavePageFilters<TFilters>(string pageName, TFilters filters) where TFilters : class
        {
            var pageFilters = new PageFilters(pageName, filters);

            var pagesFilters = (List<PageFilters>)HttpContext.Current.Session[_PAGE_FILTERS];
            if (pagesFilters == null)
            {
                HttpContext.Current.Session.Add(_PAGE_FILTERS, new List<PageFilters> { pageFilters });
            }
            else
            {
                var existingFilters = pagesFilters.SingleOrDefault(f => f.PageName == pageName);
                if (existingFilters != null)
                {
                    pagesFilters.Remove(existingFilters);
                }

                pagesFilters.Add(pageFilters);
            }
        }


        public static bool ContainsCustomKey(string key)
        {
            var composedKey = ComposeCustomValueKey(key);
            return HttpContext.Current.Session[composedKey] != null;
        }

        public static void SaveCustomValue<TValue>(string key, TValue value)
        {
            var composedKey = ComposeCustomValueKey(key);

            var existingValue = HttpContext.Current.Session[composedKey];
            if (existingValue == null)
            {
                HttpContext.Current.Session.Add(composedKey, value);
            }
            else
            {
                HttpContext.Current.Session.Remove(composedKey);
                HttpContext.Current.Session.Add(composedKey, value);
            }
        }

        public static TValue GetCustomValue<TValue>(string key)
        {
            var composedKey = ComposeCustomValueKey(key);

            var value = HttpContext.Current.Session[composedKey];
            if (value == null)
            {
                throw new KeyNotFoundException();
            }

            return (TValue)value;
        }

        public static void DeleteCustomValue(string key)
        {
            var composedKey = ComposeCustomValueKey(key);

            var value = HttpContext.Current.Session[composedKey];
            if (value != null)
            {
                HttpContext.Current.Session.Remove(composedKey);
            }
        }

        private static string ComposeCustomValueKey(string key)
        {
            return _CUSTOM_VALUES + "." + key;
        }

        public static void ClearSession()
        {
            var session = HttpContext.Current?.Session;
            if (session != null)
            {
                session.Clear();
                session.RemoveAll();
                session.Abandon();
            }
        }
    }
}