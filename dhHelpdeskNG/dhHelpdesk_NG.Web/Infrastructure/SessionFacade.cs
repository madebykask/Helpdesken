namespace DH.Helpdesk.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Web.Infrastructure.Session;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Web.Areas.Reports.Models.ReportService;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public static class SessionFacade
    {
        #region Constants

        private const string _ACTIVE_TAB = "ACTIVE_TAB";

        private const string _ACTIVE_TABS = "ACTIVE_TABS";

        private const string _CASE_TRANSLATION = "CASE_TRANSLATION";

        private const string _COMPUTER_USER_SEARCH = "COMPUTER_USER_SEARCH";

        private const string _CURRENT_BULLETINBOARD_SEARCH = "CURRENT_BULLETINBOARD_SEARCH";

        private const string _CURRENT_CALENDER_SEARCH = "CURRENT_CALENDER_SEARCH";

        private const string _CURRENT_CASESOLUTION_SEARCH = "CURRENT_CASESOLUTION_SEARCH";

        private const string _CURRENT_CASE_SEARCH = "CURRENT_CASE_SEARCH";

        private const string _CURRENT_ADVANCED_SEARCH = "CURRENT_ADVANCED_SEARCH";

        private const string _CURRENT_CUSTOMER = "CURRENT_CUSTOMER";

        private const string _CURRENT_DOCUMENT_SEARCH = "CURRENT_DOCUMENT_SEARCH";

        private const string _CURRENT_LANGUAGE = "CURRENT_LANGUAGE";

        private const string _CURRENT_LANGUAGE_CODE = "CURRENT_LANGUAGE_CODE";

        private const string _CURRENT_OPERATIONLOG_SEARCH = "CURRENT_OPERATIONLOG_SEARCH";

        

        private const string _CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH = "CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH";

        private const string _CURRENT_USER = "CURRENT_USER";

        private const string _CUSTOM_VALUES = "CUSTOM_VALUES";

        private const string _PAGE_FILTERS = "PAGE_FILTERS";

        private const string _TEXT_TRANSLATION = "TEXT_TRANSLATION";

        private const string _CURRENT_CASE_LANGUAGE_ID = "CURRENT_CASE_LANGUAGE_ID";

        private const string _TEMPORARY_VALUE = "TEMPORARY_VALUE";

        private const string _CURRENT_LOGIN_Mode = "CURRENT_LOGIN_Mode";

        private const string _CURRENT_USER_IDENTITY = "CURRENT_USER_IDENTITY";

        private const string _CASE_OVERVIEW_SETTINGS_KEY = "CASE_OVERVIEW_SETTINGS";

        private const string _ADVANCED_SEARCH_OVERVIEW_SETTINGS_KEY = "ADVANCED_SEARCH_OVERVIEW_SETTINGS";

        private const string _TZ_DETECTION_KEY = "TZ_DETECTION_RESULT";

        private const string _TZ_MSG_KEY = "TZ_MSG_RESULT";

        private const string _SHOW_ACTIVE_PRODUCT_AREAS_IN_ADMIN = "SHOW_ACTIVE_PRODUCT_AREAS_IN_ADMIN";

        private const string _SHOW_ACTIVE_ORDER_TYPES_IN_ADMIN = "SHOW_ACTIVE_ORDER_TYPES_IN_ADMIN";

        private const string _AdminUsersPageLoggedInUsersTabSelectedCustomerId = "AdminUsersPageLoggedInUsersTabSelectedCustomerId";

        private const string _AdminUsersPageLockedCasesTabSelectedCustomerId = "AdminUsersPageLockedCasesTabSelectedCustomerId";

        private const string _SHOW_ACTIVE_CASE_TYPES_IN_ADMIN = "SHOW_ACTIVE_CASE_TYPES_IN_ADMIN";

        private const string _SHOW_ACTIVE_WORKING_GROUPS_IN_ADMIN = "SHOW_ACTIVE_WORKING_GROUPS_IN_ADMIN";

        private const string _SHOW_ACTIVE_CAUSING_PARTS_IN_ADMIN = "_SHOW_ACTIVE_CAUSING_PARTS_IN_ADMIN";

        private const string _SHOW_ACTIVE_CATEGORIES_IN_ADMIN = "_SHOW_ACTIVE_CATEGORIES_IN_ADMIN";

        private const string _SHOW_ACTIVE_SUPPLIERS_IN_ADMIN = "_SHOW_ACTIVE_SUPPLIERS_IN_ADMIN";

        private const string _SHOW_ACTIVE_PRIORITIES_IN_ADMIN = "_SHOW_ACTIVE_PRIORITIES_IN_ADMIN";

        private const string _SHOW_ACTIVE_STATUSES_IN_ADMIN = "_SHOW_ACTIVE_STATUSES_IN_ADMIN";

        private const string _SHOW_ACTIVE_STATE_SECONDARIES_IN_ADMIN = "_SHOW_ACTIVE_STATE_SECONDARIES_IN_ADMIN";

        private const string _SHOW_ACTIVE_REGISTRATION_SOURCE_CUSTOMER_IN_ADMIN = "_SHOW_ACTIVE_REGISTRATION_SOURCE_CUSTOMER_IN_ADMIN";

        private const string _SHOW_ACTIVE_REGION_IN_ADMIN = "_SHOW_ACTIVE_REGION_IN_ADMIN";

        private const string _SHOW_ACTIVE_DEPARTMENT_IN_ADMIN = "_SHOW_ACTIVE_DEPARTMENT_IN_ADMIN";

        private const string _SHOW_ACTIVE_OU_IN_ADMIN = "_SHOW_ACTIVE_OU_IN_ADMIN";

        private const string _SHOW_ACTIVE_STANDARDTEXTS_IN_ADMIN = "_SHOW_ACTIVE_STANDARDTEXTS_IN_ADMIN";

        private const string _SHOW_ACTIVE_FINISHINGCAUSES_IN_ADMIN = "_SHOW_ACTIVE_FINISHINGCAUSES_IN_ADMIN";

        private const string _SHOW_ACTIVE_DAILYREPORTSUBJECTS_IN_ADMIN = "_SHOW_ACTIVE_DAILYREPORTSUBJECTS_IN_ADMIN";

        private const string _SHOW_ACTIVE_OPERATIONOBJECTS_IN_ADMIN = "_SHOW_ACTIVE_OPERATIONOBJECTS_IN_ADMIN";

        private const string _SHOW_ACTIVE_OPERATIONLOGCATEGORIES_IN_ADMIN = "_SHOW_ACTIVE_OPERATIONLOGCATEGORIES_IN_ADMIN";

        private const string _SHOW_ACTIVE_EMAILGROUPS_IN_ADMIN = "_SHOW_ACTIVE_EMAILGROUPS_IN_ADMIN";

        private const string _SHOW_ACTIVE_PROGRAMS_IN_ADMIN = "_SHOW_ACTIVE_PROGRAMS_IN_ADMIN";

        private const string _REPORT_SERVICE_SESSION_MODEL = "REPORT_SERVICE_SESSION_MODEL";

        private const string _IS_CASE_DATA_CHANGED = "Is_CASE_DATA_CHANGED";

        #endregion

        #region Public Properties

        public static bool IsCaseDataChanged
        {
            get
            {
                if (HttpContext.Current.Session[_IS_CASE_DATA_CHANGED] != null)
                    return (bool)HttpContext.Current.Session[_IS_CASE_DATA_CHANGED];
                else
                    return false;
            }
            set
            {
                if (HttpContext.Current.Session[_IS_CASE_DATA_CHANGED] == null)
                    HttpContext.Current.Session.Add(_IS_CASE_DATA_CHANGED, value);
                else
                    HttpContext.Current.Session[_IS_CASE_DATA_CHANGED] = value;
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
                {
                    HttpContext.Current.Session.Add(_ACTIVE_TAB, value);
                }
                else
                {
                    HttpContext.Current.Session[_ACTIVE_TAB] = value;
                }
            }
        }

        public static string TemporaryValue
        {
            get
            {
                return (string)HttpContext.Current.Session[_TEMPORARY_VALUE];
            }
            set
            {
                if (HttpContext.Current.Session[_TEMPORARY_VALUE] == null)
                {
                    HttpContext.Current.Session.Add(_TEMPORARY_VALUE, value);
                }
                else
                {
                    HttpContext.Current.Session[_TEMPORARY_VALUE] = value;
                }
            }
        }

        //public static IList<CaseFieldSettingsForTranslation> CaseTranslation
        //{
        //    get
        //    {
        //        return (IList<CaseFieldSettingsForTranslation>)HttpContext.Current.Session[_CASE_TRANSLATION];
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Session[_CASE_TRANSLATION] == null)
        //        {
        //            HttpContext.Current.Session.Add(_CASE_TRANSLATION, value);
        //        }
        //        else
        //        {
        //            HttpContext.Current.Session[_CASE_TRANSLATION] = value;
        //        }
        //    }
        //}

        public static int CurrentCaseLanguageId
        {
            get
            {
                return (int)HttpContext.Current.Session[_CURRENT_CASE_LANGUAGE_ID];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CASE_LANGUAGE_ID] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_CASE_LANGUAGE_ID, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_CASE_LANGUAGE_ID] = value;
                }
            }
        }

        public static BulletinBoardSearch CurrentBulletinBoardSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] == null)
                {
                    return null;
                }
                return (BulletinBoardSearch)HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_BULLETINBOARD_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_BULLETINBOARD_SEARCH] = value;
                }
            }
        }

        public static CalendarSearch CurrentCalenderSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] == null)
                {
                    return null;
                }
                return (CalendarSearch)HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_CALENDER_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_CALENDER_SEARCH] = value;
                }
            }
        }

        public static CaseSearchModel CurrentCaseSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CASE_SEARCH] == null)
                {
                    return null;
                }
                return (CaseSearchModel)HttpContext.Current.Session[_CURRENT_CASE_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CASE_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_CASE_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_CASE_SEARCH] = value;
                }
            }
        }

        public static CaseSearchModel CurrentAdvancedSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_ADVANCED_SEARCH] == null)
                {
                    return null;
                }
                return (CaseSearchModel)HttpContext.Current.Session[_CURRENT_ADVANCED_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_ADVANCED_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_ADVANCED_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_ADVANCED_SEARCH] = value;
                }
            }
        }

        public static CaseSolutionSearch CurrentCaseSolutionSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] == null)
                {
                    return null;
                }
                return (CaseSolutionSearch)HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_CASESOLUTION_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_CASESOLUTION_SEARCH] = value;
                }
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
                {
                    HttpContext.Current.Session.Add(_COMPUTER_USER_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_COMPUTER_USER_SEARCH] = value;
                }
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
                {
                    HttpContext.Current.Session.Add(_CURRENT_CUSTOMER, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_CUSTOMER] = value;
                }
            }
        }

        public static DocumentSearch CurrentDocumentSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] == null)
                {
                    return null;
                }
                return (DocumentSearch)HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_DOCUMENT_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_DOCUMENT_SEARCH] = value;
                }
            }
        }

        public static int CurrentLanguageId
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(_CURRENT_LANGUAGE);
                if (cookie != null)
                {
                    return Convert.ToInt32(cookie.Value);
                }

                if (CurrentUser != null)
                {
                    return CurrentUser.LanguageId;
                }

                return 0;
            }

            set
            {
                if (HttpContext.Current.Request.Cookies[_CURRENT_LANGUAGE] == null)
                {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie(_CURRENT_LANGUAGE, value.ToString()));
                }
                else
                {
                    HttpContext.Current.Response.Cookies[_CURRENT_LANGUAGE].Value = value.ToString();
                }
            }
        }

        public static string CurrentLanguageCode
        {
            get
            {
                return (string)HttpContext.Current.Session[_CURRENT_LANGUAGE_CODE];
            }
            set
            {
                if(HttpContext.Current.Session[_CURRENT_LANGUAGE_CODE] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_LANGUAGE_CODE, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_LANGUAGE_CODE] = value;
                }
            }
        }

        public static OperationLogSearch CurrentOperationLogSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] == null)
                {
                    return null;
                }
                return (OperationLogSearch)HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_OPERATIONLOG_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_OPERATIONLOG_SEARCH] = value;
                }
            }
        }

      

        public static InvoiceArticleProductAreaSelectedFilter CurrentInvoiceArticleProductAreaSearch
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH] == null)
                {
                    return null;
                }
                return (InvoiceArticleProductAreaSelectedFilter)HttpContext.Current.Session[_CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_INVOICE_ARTICLE_PRODUCTAREA_SEARCH] = value;
                }
            }
        }

        public static UserOverview CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                {
                    return null;
                }
                return (UserOverview)HttpContext.Current.Session[_CURRENT_USER];
            }
            set
            {
                if (HttpContext.Current.Session[_CURRENT_USER] == null)
                {
                    HttpContext.Current.Session.Add(_CURRENT_USER, value);
                }
                else
                {
                    HttpContext.Current.Session[_CURRENT_USER] = value;
                }
            }
        }

        public static LoginMode CurrentLoginMode
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENT_LOGIN_Mode] == null)
                {
                    return LoginMode.None;
                }

                return (LoginMode) HttpContext.Current.Session[_CURRENT_LOGIN_Mode];
            }
            set
            {
                HttpContext.Current.Session[_CURRENT_LOGIN_Mode] = value;
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

        //public static IList<Text> TextTranslation
        //{
        //    get
        //    {
        //        return (IList<Text>)HttpContext.Current.Session[_TEXT_TRANSLATION];
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Session[_TEXT_TRANSLATION] == null)
        //        {
        //            HttpContext.Current.Session.Add(_TEXT_TRANSLATION, value);
        //        }
        //        else
        //        {
        //            HttpContext.Current.Session[_TEXT_TRANSLATION] = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Holds settings for "Case overview" grid
        /// </summary>
        public static GridSettingsModel CaseOverviewGridSettings
        {
            get
            {
                return (GridSettingsModel)HttpContext.Current.Session[_CASE_OVERVIEW_SETTINGS_KEY];
            }

            set
            {
                if (HttpContext.Current.Session[_CASE_OVERVIEW_SETTINGS_KEY] == null)
                {
                    HttpContext.Current.Session.Add(_CASE_OVERVIEW_SETTINGS_KEY, value);
                }
                else
                {
                    HttpContext.Current.Session[_CASE_OVERVIEW_SETTINGS_KEY] = value;
                }
            }
        }

        public static GridSettingsModel AdvancedSearchOverviewGridSettings
        {
            get
            {
                return (GridSettingsModel)HttpContext.Current.Session[_ADVANCED_SEARCH_OVERVIEW_SETTINGS_KEY];
            }

            set
            {
                if (HttpContext.Current.Session[_ADVANCED_SEARCH_OVERVIEW_SETTINGS_KEY] == null)
                {
                    HttpContext.Current.Session.Add(_ADVANCED_SEARCH_OVERVIEW_SETTINGS_KEY, value);
                }
                else
                {
                    HttpContext.Current.Session[_ADVANCED_SEARCH_OVERVIEW_SETTINGS_KEY] = value;
                }
            }
        }

        public static TimeZoneAutodetectResult TimeZoneDetectionResult
        {
            get
            {
                if (HttpContext.Current.Session[_TZ_DETECTION_KEY] != null)
                {
                    return (TimeZoneAutodetectResult)HttpContext.Current.Session[_TZ_DETECTION_KEY];
                }

                return TimeZoneAutodetectResult.None;
            }

            set
            {
                SaveSetKeyValue(_TZ_DETECTION_KEY, value);
            }
        }

        public static bool ShowOnlyActiveProductAreasInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_PRODUCT_AREAS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_PRODUCT_AREAS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_PRODUCT_AREAS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveOrderTypesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_ORDER_TYPES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_ORDER_TYPES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_ORDER_TYPES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveCaseTypesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_CASE_TYPES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_CASE_TYPES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_CASE_TYPES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveWorkingGroupsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_WORKING_GROUPS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_WORKING_GROUPS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_WORKING_GROUPS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveCausingPartsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_CAUSING_PARTS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_CAUSING_PARTS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_CAUSING_PARTS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveCategoriesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_CATEGORIES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_CATEGORIES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_CATEGORIES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveSuppliersInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_SUPPLIERS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_SUPPLIERS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_SUPPLIERS_IN_ADMIN, value);
            }
        }


        public static bool ShowOnlyActivePrioritiesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_PRIORITIES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_PRIORITIES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_PRIORITIES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveStatusesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_STATUSES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_STATUSES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_STATUSES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveStateSecondariesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_STATE_SECONDARIES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_STATE_SECONDARIES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_STATE_SECONDARIES_IN_ADMIN, value);
            }
        }


        public static bool ShowOnlyActiveRegistrationSourceCustomerInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_REGISTRATION_SOURCE_CUSTOMER_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_REGISTRATION_SOURCE_CUSTOMER_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_REGISTRATION_SOURCE_CUSTOMER_IN_ADMIN, value);
            }
        }


        public static bool ShowOnlyActiveRegionInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_REGION_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_REGION_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_REGION_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveDepartmentInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_DEPARTMENT_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_DEPARTMENT_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_DEPARTMENT_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveOUInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_OU_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_OU_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_OU_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveStandardTextsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_STANDARDTEXTS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_STANDARDTEXTS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_STANDARDTEXTS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveFinishingCausesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_FINISHINGCAUSES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_FINISHINGCAUSES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_FINISHINGCAUSES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveDailyReportSubjectsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_DAILYREPORTSUBJECTS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_DAILYREPORTSUBJECTS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_DAILYREPORTSUBJECTS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveOperationObjectsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_OPERATIONOBJECTS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_OPERATIONOBJECTS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_OPERATIONOBJECTS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveOperationLogCategoriesInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_OPERATIONLOGCATEGORIES_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_OPERATIONLOGCATEGORIES_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_OPERATIONLOGCATEGORIES_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveEMailGroupsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_EMAILGROUPS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_EMAILGROUPS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_EMAILGROUPS_IN_ADMIN, value);
            }
        }

        public static bool ShowOnlyActiveProgramsInAdmin
        {
            get
            {
                if (HttpContext.Current.Session[_SHOW_ACTIVE_PROGRAMS_IN_ADMIN] != null)
                {
                    return (bool)HttpContext.Current.Session[_SHOW_ACTIVE_PROGRAMS_IN_ADMIN];
                }

                return true;
            }

            set
            {
                SaveSetKeyValue(_SHOW_ACTIVE_PROGRAMS_IN_ADMIN, value);
            }
        }

        public static bool WasTimeZoneMessageDisplayed
        {
            get
            {
                return HttpContext.Current.Session[_TZ_MSG_KEY] != null && (bool)HttpContext.Current.Session[_TZ_MSG_KEY];
            }

            set
            {
                SaveSetKeyValue(_TZ_MSG_KEY, value);
            }
        }

        public static int? AdminUsersPageLoggedInUsersTabSelectedCustomerId
        {
            get
            {
                if (HttpContext.Current.Session[_AdminUsersPageLoggedInUsersTabSelectedCustomerId] != null)
                {
                    return (int?)HttpContext.Current.Session[_AdminUsersPageLoggedInUsersTabSelectedCustomerId];
                }

                return null;
            }

            set
            {
                SaveSetKeyValue(_AdminUsersPageLoggedInUsersTabSelectedCustomerId, value);
            }
        }

        public static int? AdminUsersPageLockedCasesTabSelectedCustomerId
        {
            get
            {
                if (HttpContext.Current.Session[_AdminUsersPageLockedCasesTabSelectedCustomerId] != null)
                {
                    return (int?)HttpContext.Current.Session[_AdminUsersPageLockedCasesTabSelectedCustomerId];
                }

                return null;
            }

            set
            {
                SaveSetKeyValue(_AdminUsersPageLockedCasesTabSelectedCustomerId, value);
            }
        }

        public static ReportServiceSessionModel ReportService
        {
            get
            {
                if (HttpContext.Current.Session[_REPORT_SERVICE_SESSION_MODEL] == null)
                {
                    return null;
                }
                return (ReportServiceSessionModel)HttpContext.Current.Session[_REPORT_SERVICE_SESSION_MODEL];
            }
            set
            {
                if (HttpContext.Current.Session[_REPORT_SERVICE_SESSION_MODEL] == null)
                {
                    HttpContext.Current.Session.Add(_REPORT_SERVICE_SESSION_MODEL, value);
                }
                else
                {
                    HttpContext.Current.Session[_REPORT_SERVICE_SESSION_MODEL] = value;
                }
            }
        }



        private static void SaveSetKeyValue(string key, object value)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                HttpContext.Current.Session.Add(key, value);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static bool ContainsCustomKey(string key)
        {
            var composedKey = ComposeCustomValueKey(key);
            return HttpContext.Current.Session[composedKey] != null;
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

        public static string FindActiveTab(string topic)
        {
            var activeTabs = (List<ActiveTab>)HttpContext.Current.Session[_ACTIVE_TABS];
            if (activeTabs == null)
            {
                return null;
            }

            var activeTab = activeTabs.SingleOrDefault(t => t.Topic == topic);
            return activeTab == null ? null : activeTab.Tab;
        }

        public static TFilters FindPageFilters<TFilters>(string pageName) where TFilters : class
        {
            var pagesFilters = (List<PageFilters>)HttpContext.Current.Session[_PAGE_FILTERS];
            if (pagesFilters == null)
            {
                return null;
            }

            var pageFilters = pagesFilters.SingleOrDefault(f => f.PageName == pageName);
            if (pageFilters == null)
            {
                return null;
            }

            return (TFilters)pageFilters.Filters;
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

        public static void SaveActiveTab(string topic, string tab)
        {
            var activeTab = new ActiveTab(topic, tab);

            var activeTabs = (List<ActiveTab>)HttpContext.Current.Session[_ACTIVE_TABS];
            if (activeTabs == null)
            {
                activeTabs = new List<ActiveTab> { activeTab };
                HttpContext.Current.Session.Add(_ACTIVE_TABS, activeTabs);
            }
            else
            {
                var existingActiveTab = activeTabs.SingleOrDefault(t => t.Topic == topic);
                if (existingActiveTab != null)
                {
                    activeTabs.Remove(existingActiveTab);
                }

                activeTabs.Add(activeTab);
            }
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

        #endregion

        #region Common Methods and Properties

        public static string SessionId
        {
            get
            {
                return HttpContext.Current?.Session?.SessionID;
            }
        }

        public static void ClearSession(bool abandon = false)
        {
            var session = HttpContext.Current?.Session;
            if (session != null)
            {
                session.Clear();
                if (abandon)
                {
                    session.RemoveAll();
                    session.Abandon();
                }
            }
        }

        #endregion

        #region Methods

        private static string ComposeCustomValueKey(string key)
        {
            return _CUSTOM_VALUES + "." + key;
        }

        #endregion
    }
}