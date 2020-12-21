using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.ADFS.Input;
using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.BusinessData.Models.Error;
using DH.Helpdesk.BusinessData.Models.Language.Output;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Extensions;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using DH.Helpdesk.SelfService.Models;
using DH.Helpdesk.Services.Services;
using log4net;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    public class BaseController : Controller
    {
        const string DEFAULT_ANONYMOUS_USER_ID = "AnonymousUser";

        private readonly ILog _log = LogManager.GetLogger(typeof(BaseController));
        private readonly ISelfServiceConfigurationService _configurationService;
        private readonly IMasterDataService _masterDataService;
        private bool _userOrCustomerChanged;
        private List<LanguageOverview> _languages;

        protected ISelfServiceConfigurationService ConfigurationService => _configurationService;


        public BaseController(ISelfServiceConfigurationService configurationService,
            IMasterDataService masterDataService,
            ICaseSolutionService caseSolutionService)
        {
            _configurationService = configurationService;
            _masterDataService = masterDataService;
        }


        //called before a controller action is executed, that is before ~/HomeController/index 
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Debugger.Launch();

            if (!CheckUserAccessToUrl(filterContext))
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError("Url is not valid!");
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                return;
            }

            var customerId = -1;
            TempData["ShowLanguageSelect"] = true;
            SessionFacade.LastError = null;
            var lastError = new ErrorModel(string.Empty);
            _userOrCustomerChanged = false;

            var appSettings = ConfigurationService.AppSettings;
            var loginMode = appSettings.LoginMode;
 
            var res = SetCustomer(filterContext, out lastError);

            //todo: move to OnSessionEnd
            //if (!res && isSsoMode && federatedAuthenticationSettings.LogoutCustomerOnSessionExpire)
            //{
            //    ManualDependencyResolver.Get<IFederatedAuthenticationService>().SignOut(Request.Url.AbsolutePath);
            //}

            if (!res && lastError != null)
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError(lastError);
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                return;
            }
            customerId = SessionFacade.CurrentCustomerID;

            SetLanguage();


            if (SessionFacade.CurrentUserIdentity == null)
            {
                if (loginMode == LoginMode.SSO)
                {
                    var userIdentity = TrySSOLogin(User, out lastError);
                    if (lastError != null)
                    {
                        SessionFacade.UserHasAccess = false;
                        ErrorGenerator.MakeError(lastError);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }

                    SessionFacade.CurrentSystemUser = userIdentity.UserId;
                    SessionFacade.CurrentUserIdentity = userIdentity;
                    SessionFacade.UserHasAccess = true;
                } // SSO Login

                else if (loginMode == LoginMode.Windows)
                {

                    SessionFacade.CurrentCoWorkers = null;
                    var userIdentity = TryWindowsLogin(customerId, out lastError);
                    if (lastError != null)
                    {
                        SessionFacade.UserHasAccess = false;
                        ErrorGenerator.MakeError(lastError);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }

                    SessionFacade.CurrentSystemUser = userIdentity.UserId;
                    SessionFacade.CurrentUserIdentity = userIdentity;
                    SessionFacade.UserHasAccess = true;
                }
                else if (loginMode == LoginMode.Microsoft)
                {

                    SessionFacade.CurrentCoWorkers = null;
                    var userIdentity = TryMicrosoftLogin(customerId, out lastError);
                    if (lastError != null)
                    {
                        SessionFacade.UserHasAccess = false;
                        ErrorGenerator.MakeError(lastError);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }

                    SessionFacade.CurrentSystemUser = userIdentity.UserId;
                    SessionFacade.CurrentUserIdentity = userIdentity;
                    SessionFacade.UserHasAccess = true;
                }
                else
                {
                    SessionFacade.CurrentCoWorkers = null;
                    var userIdentity = TryAnonymousLogin(customerId, out lastError);
                    if (lastError != null)
                    {
                        SessionFacade.UserHasAccess = false;
                        ErrorGenerator.MakeError(lastError);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }

                    SessionFacade.CurrentSystemUser = userIdentity.UserId;
                    SessionFacade.CurrentUserIdentity = userIdentity;
                    SessionFacade.UserHasAccess = true;
                }

                _userOrCustomerChanged = true;
            } //User Session was null


            if (_userOrCustomerChanged)
            {
                SessionFacade.CurrentCoWorkers = null;

                // load user info according database info (tblComputerUser)
                LoadUserInfo();

                //load user info from tblUsers if such user exist
                LoadLocalUserInfo();
            }

            //LogWithContext("OnActionExecuting: user and customer has been loaded.");

            if (SessionFacade.CurrentCustomer.RestrictUserToGroupOnExternalPage)
            {
                SetUserRestriction(customerId, out lastError);
                if (lastError != null)
                {
                    SessionFacade.UserHasAccess = false;
                    ErrorGenerator.MakeError(lastError);
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                    return;
                }
            }

            ViewBag.IsLineManagerApplication = IsLineManagerApplication();
            ViewBag.ApplicationType = _configurationService.AppSettings.ApplicationType;

            SetTextTranslation(filterContext);
        }

        private bool SetCustomer(ActionExecutingContext filterContext, out ErrorModel lastMessage)
        {
            var customerId = -1;
            lastMessage = null;
            var sessionCustomerId = SessionFacade.CurrentCustomer != null ? SessionFacade.CurrentCustomer.Id : -1;
            customerId = RetrieveCustomer(filterContext, sessionCustomerId);

            if (SessionFacade.CurrentCustomer == null && customerId == -1)
            {
                //filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                lastMessage = new ErrorModel(101, "Customer Id can't be empty!");
                return false;
            }

            // CustomerId has been changed by user
            if ((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId && customerId != -1) ||
                (SessionFacade.CurrentCustomer == null && customerId != -1))
            {
                _userOrCustomerChanged = true;
                var newCustomer = _masterDataService.GetCustomer(customerId);
                SessionFacade.CurrentCustomer = newCustomer;

                // Customer changed then clear sessions
                SessionFacade.CurrentCoWorkers = null;
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                SessionFacade.UserHasAccess = false;
                lastMessage = new ErrorModel(101, string.Format("Customer is not valid!", customerId));
                return false;
            }

            SessionFacade.UserHasAccess = true;
            SessionFacade.CurrentCustomerID = SessionFacade.CurrentCustomer.Id;

            //set customerId cookie
            ControllerContext.HttpContext.SetCustomerIdCookie(SessionFacade.CurrentCustomer.Id);

            return true;
        }

        private int RetrieveCustomer(ActionExecutingContext filterContext, int sessionCustomerId)
        {
            var ret = -1;

            var passedCustomerId = Request.QueryString["customerId"];

            if (!string.IsNullOrEmpty(passedCustomerId))
            {
                int tempId = 0;
                if (int.TryParse(passedCustomerId, out tempId))
                {
                    ret = tempId;
                }
                else
                {
                    ErrorGenerator.MakeError("Customer Id not valid!", 105);
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                    return -1;
                }
            }

            if (filterContext.ActionParameters.Keys.Contains("id", StringComparer.OrdinalIgnoreCase))
            {
                var _id = filterContext.ActionParameters["id"];
                if (_id != null)
                {
                    var _strId = _id.ToString();
                    if (!string.IsNullOrEmpty(_strId) && GuidHelper.IsGuid(_strId))
                    {
                        var guid = new Guid(_strId);
                        int? tempCustomerId = _masterDataService.GetCustomerIdByEMailGUID(guid);
                        if (tempCustomerId != null && tempCustomerId > 0)
                            ret = tempCustomerId.Value;
                    }
                }
            }

            //try to get customerId from query string
            if (sessionCustomerId < 0 && ret < 0)
            {
                var val = Request.QueryString["customerId"];

                int paramCustomerId = -1;
                int.TryParse(val, out paramCustomerId);

                // if failed from qs try to load from session cookie. Case - when was signed out from page without customerId in url
                if (paramCustomerId <= 0)
                {
                    paramCustomerId = filterContext.HttpContext.GetCustomerIdFromCookie();
                }

                if (paramCustomerId <= 0)
                {
                    ErrorGenerator.MakeError("Customer Id not valid!", 105);
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                    return -1;
                }

                if (paramCustomerId > 0)
                    ret = paramCustomerId;
            }

            return ret;
        }

        private void SetLanguage()
        {
            if (SessionFacade.AllLanguages == null)
                SessionFacade.AllLanguages = GetActiveLanguages();

            var requestLanguageId = Request.QueryString.Get("language");

            if (!string.IsNullOrEmpty(requestLanguageId))
            {
                var langId = -1;
                var languages = GetActiveLanguages();
                var lang = languages.FirstOrDefault(l => l.LanguageId.Equals(requestLanguageId, StringComparison.OrdinalIgnoreCase));

                if (lang != null)
                {
                    langId = lang.Id;
                    SessionFacade.CurrentLanguageId = langId;
                }
            }
            else
            {
                if (SessionFacade.CurrentCustomer != null & SessionFacade.CurrentLanguageId == 0)
                    SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            }
        }

        private void LoadUserInfo()
        {
            if (SessionFacade.CurrentCustomer != null &&
                SessionFacade.CurrentUserIdentity != null &&
                string.IsNullOrEmpty(SessionFacade.CurrentUserIdentity.FirstName) &&
                string.IsNullOrEmpty(SessionFacade.CurrentUserIdentity.LastName) &&
                string.IsNullOrEmpty(SessionFacade.CurrentUserIdentity.Email))
            {
                var dbInitiator = _masterDataService.GetInitiatorByUserId(SessionFacade.CurrentUserIdentity.UserId, SessionFacade.CurrentCustomer.Id);
                if (dbInitiator != null)
                {
                    SessionFacade.CurrentUserIdentity.FirstName = dbInitiator.FirstName;
                    SessionFacade.CurrentUserIdentity.LastName = dbInitiator.LastName;
                    SessionFacade.CurrentUserIdentity.Email = dbInitiator.Email;
                }
            }
        }

        private void LoadLocalUserInfo()
        {
            //_log.Debug("LoadLocalUserInfo: check if local user information can be loaded.");

            if (SessionFacade.CurrentCustomer != null &&
                SessionFacade.CurrentUserIdentity != null)
            {
                var userNames = SessionFacade.CurrentUserIdentity.UserId.Split(@"\");
                var userNamesToCheck = new List<string>
                {
                    SessionFacade.CurrentUserIdentity.UserId
                };
                if (!string.IsNullOrWhiteSpace(SessionFacade.CurrentUserIdentity.Domain)) userNamesToCheck.Add($@"{SessionFacade.CurrentUserIdentity.Domain}\{SessionFacade.CurrentUserIdentity.UserId}");
                if (userNames.Length > 1) userNamesToCheck.Add(userNames[userNames.Length - 1]);

                foreach (var userName in userNamesToCheck)
                {
                    if (string.IsNullOrWhiteSpace(userName)) continue;

                    var user = _masterDataService.GetUserForLogin(userName);
                    if (user != null && _masterDataService.IsCustomerUser(SessionFacade.CurrentCustomer.Id, user.Id))
                    {
                        SessionFacade.CurrentLocalUser = user;
                        return;
                    }
                }
            }
            SessionFacade.CurrentLocalUser = null;
        }


        private UserIdentity TrySSOLogin(System.Security.Principal.IPrincipal user, out ErrorModel lastError)
        {
            lastError = null;
            var userIdentity = new UserIdentity();

            ClaimsPrincipal principal = user as ClaimsPrincipal;
            if (principal != null)
            {
                string claimData = "";
                bool isFirst = true;

                var claimDomain = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimDomain);
                var claimUserId = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimUserId);
                var claimEmployeeNumber = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimEmployeeNumber);
                var claimFirstName = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimFirstName);
                var claimLastName = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimLastName);
                var claimEmail = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimEmail);
                var claimPhone = AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimPhone);

                foreach (Claim claim in principal.Claims)
                {
                    var claimTypeArray = claim.Type.Split('/');
                    var pureType = claimTypeArray.LastOrDefault();
                    var value = claim.Value;

                    if (isFirst)
                        claimData = "[" + ((pureType != null) ? pureType.ToString() : "Undefined") + ": " + value.ToString() + "]";
                    else
                        claimData = claimData + " , [" + ((pureType != null) ? pureType.ToString() : "Undefined") + ": " + value.ToString() + "]";

                    isFirst = false;

                    if (pureType != null)
                    {
                        pureType = pureType.CleanSpaceAndLowStr();

                        if (pureType == claimDomain)
                            userIdentity.Domain = value;

                        if (pureType == claimUserId)
                            userIdentity.UserId = value;

                        if (pureType == claimEmployeeNumber)
                            userIdentity.EmployeeNumber = value;

                        if (pureType == claimFirstName)
                            userIdentity.FirstName = value;

                        if (pureType == claimLastName)
                            userIdentity.LastName = value;

                        if (pureType == claimEmail)
                            userIdentity.Email = value;

                        if (pureType == claimPhone)
                            userIdentity.Phone = value;
                    }
                }

                var defaultUserId = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultUserId);
                if (!string.IsNullOrEmpty(defaultUserId))
                    userIdentity.UserId = defaultUserId;

                var defaultEmployeeNumber = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber);
                if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                    userIdentity.EmployeeNumber = defaultEmployeeNumber;

                var netId = principal.Identity.Name;
                var ssoLog = new NewSSOLog()
                {
                    ApplicationId = ConfigurationManager.AppSettings[AppSettingsKey.ApplicationId].ToString(),
                    NetworkId = netId,
                    ClaimData = claimData,
                    CreatedDate = DateTime.Now
                };

                if (AppConfigHelper.GetAppSetting(AppSettingsKey.SSOLog) == BooleanString.TRUE &&
                    string.IsNullOrEmpty(SessionFacade.CurrentSystemUser))
                    _masterDataService.SaveSSOLog(ssoLog);

                if (string.IsNullOrEmpty(userIdentity.UserId))
                {
                    lastError = new ErrorModel(107, "You don't have access to the portal. (User Id is not specified)");
                    return userIdentity;
                }
            }

            return userIdentity;
        }

        private UserIdentity TryAnonymousLogin(int customerId, out ErrorModel lastError)
        {
            lastError = null;
            UserIdentity userIdentity = null;
            var employeeNum = string.Empty;
            string userId = DEFAULT_ANONYMOUS_USER_ID; // shall we get login user? global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var defaultUserId = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultUserId);
            if (!string.IsNullOrEmpty(defaultUserId))
                userId = defaultUserId;

            var defaultEmployeeNumber = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber);
            if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                employeeNum = defaultEmployeeNumber;

            string userDomain = string.Empty;
            var initiator = _masterDataService.GetInitiatorByUserId(userId, customerId);
            userIdentity = new UserIdentity()
            {
                UserId = userId,
                Domain = userDomain,
                FirstName = initiator?.FirstName,
                LastName = initiator?.LastName,
                EmployeeNumber = employeeNum,
                Phone = initiator?.Phone,
                Email = initiator?.Email
            };

            return userIdentity;
        }

        private UserIdentity TryWindowsLogin(int customerId, out ErrorModel lastError)
        {
            lastError = null;
            UserIdentity userIdentity = null;
            //var user = System.Security.Principal.WindowsIdentity.GetCurrent(); // do not use WindowsIdentity!
            var user = HttpContext.User.Identity;
            var employeeNum = string.Empty;

            string fullName = user.Name;
            string userId = fullName.GetUserFromAdPath();

            var defaultUserId = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultUserId);
            if (!string.IsNullOrEmpty(defaultUserId))
                userId = defaultUserId;

            var defaultEmployeeNumber = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber);
            if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                employeeNum = defaultEmployeeNumber;

            string userDomain = fullName.GetDomainFromAdPath();
            var initiator = _masterDataService.GetInitiatorByUserId(userId, customerId);
            userIdentity = new UserIdentity()
            {
                UserId = userId,
                Domain = userDomain,
                FirstName = initiator?.FirstName,
                LastName = initiator?.LastName,
                EmployeeNumber = employeeNum,
                Phone = initiator?.Phone,
                Email = initiator?.Email
            };

            return userIdentity;
        }

        private UserIdentity TryMicrosoftLogin(int customerId, out ErrorModel lastError)
        {
            lastError = null;
            UserIdentity userIdentity = null;

            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            var user = HttpContext.User.Identity;
            var employeeNum = string.Empty;

            string fullName = userClaims?.FindFirst("name")?.Value; //user.Name; TODO: Check why name is empty for user, but not for claims
            string mailAddress = userClaims?.FindFirst("preferred_username")?.Value;
            var initiator = _masterDataService.GetInitiatorByMail(mailAddress, customerId);            

            //var defaultUserId = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultUserId);
            //if (!string.IsNullOrEmpty(defaultUserId))
            //    userId = defaultUserId;

            var defaultEmployeeNumber = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber);
            if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                employeeNum = defaultEmployeeNumber;

            //string userDomain = fullName.GetDomainFromAdPath();
            //var initiator = _masterDataService.GetInitiatorByUserId(userId, customerId);
            userIdentity = new UserIdentity()
            {
                UserId = initiator?.UserId ?? mailAddress,
                Domain = "",
                FirstName = initiator?.FirstName,
                LastName = initiator?.LastName,
                EmployeeNumber = employeeNum,
                Phone = initiator?.Phone,
                Email = initiator?.Email ?? mailAddress
            };

            return userIdentity;
        }

        private void SetUserRestriction(int customerId, out ErrorModel lastError)
        {
            lastError = null;
            var employeeNumber = SessionFacade.CurrentUserIdentity.EmployeeNumber;

            if (!UserBelongedToCurrentCustomer(employeeNumber))
            {
                SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();
                lastError = new ErrorModel(103, "You don't have access to the portal. (User is not manager for country)");
                return;
            }

            if (SessionFacade.CurrentCoWorkers == null ||
                (SessionFacade.CurrentCoWorkers != null && SessionFacade.CurrentCoWorkers.Count == 0))
            {
                if (string.IsNullOrEmpty(employeeNumber))
                {
                    lastError = new ErrorModel(104, "You don't have access to the portal. (Employee Number is not specified)");
                    return;
                }

                var useApi = SessionFacade.CurrentCustomer.FetchDataFromApiOnExternalPage;
                var apiCredential = WebApiConfig.GetAmApiInfo();
                var employee = _masterDataService.GetEmployee(customerId, employeeNumber, useApi, apiCredential);

                if (employee != null && employee.IsManager)
                {
                    SessionFacade.CurrentCoWorkers = employee.Subordinates;
                }
                else
                {
                    SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();
                    lastError = new ErrorModel(102, "You don't have access to the portal. (User is not manager)");
                    return;
                }
            }
        }

        public ActionResult ChangeLanguage(string language, string currentUrl, string lastParams)
        {
            if (SessionFacade.AllLanguages != null)
            {
                List<LanguageOverview> allLang = SessionFacade.AllLanguages.ToList();
                var lId = allLang.Where(a => a.LanguageId == language).Select(a => a.Id).SingleOrDefault();
                SessionFacade.CurrentLanguageId = lId;
            }

            currentUrl += "?language=" + language;

            var allParam = lastParams.Split('&');
            foreach (var param in allParam)
            {
                if ((!string.IsNullOrEmpty(param)) && (!param.ToLower().Contains("language=")))
                    currentUrl += "&" + param;
            }

            return Redirect(currentUrl);
        }

        protected bool UserBelongedToCurrentCustomer(string employeeNumber)
        {
            /*This is IKEA specific condition*/
            if (SessionFacade.CurrentCustomer != null && SessionFacade.CurrentUserIdentity != null &&
                !string.IsNullOrEmpty(employeeNumber))
            {
                var config = (ECT.FormLib.Configurable.AccessManagment)ConfigurationManager.GetSection("formLibConfigurable/accessManagment");
                var country = config.Countries.Where(x => x.HelpdeskCustomerId == SessionFacade.CurrentCustomer.Id.ToString()).FirstOrDefault();

                if (country == null || (country != null && !employeeNumber.StartsWith(country.EmployeePrefix)))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private List<LanguageOverview> GetActiveLanguages()
        {
            if (_languages == null)
            {
                _languages = _masterDataService.GetLanguages()
                    .Where(l => l.IsActive == 1)
                    .Select(la => new LanguageOverview { Id = la.Id, IsActive = la.IsActive, LanguageId = la.LanguageID, Name = la.Name })
                    .OrderBy(l => l.Name)
                    .ToList();
            }
            return _languages;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext) //called after a controller action is executed, that is after ~/UserController/index 
        {
            SetMasterPageModel(filterContext);
            base.OnActionExecuted(filterContext);

            // Last correct url used for back from Error page
            if (SessionFacade.LastError == null)
                SessionFacade.LastCorrectUrl =
                    filterContext.HttpContext.Request.Url.AbsoluteUri;
        }

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            return this.RenderViewToString(viewName, model, partial);
        }

        protected ApplicationType CurrentApplicationType
        {
            get
            {
                var appType = _configurationService.AppSettings.ApplicationType;
                return EnumHelper.Parse<ApplicationType>(appType);
            }
        }

        protected bool IsLineManagerApplication()
        {
            var applicationType = _configurationService.AppSettings.ApplicationType;
            return ApplicationTypes.LineManager.Equals(applicationType, StringComparison.OrdinalIgnoreCase);
        }

        protected ActionResult RedirectToErrorPage()
        {
            return RedirectToAction("Index", "Error");
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer ?? _masterDataService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel
            {
                Languages = _masterDataService.GetLanguages(),
                SelectedLanguageId = SessionFacade.CurrentLanguageId
            };

            if (SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = _masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }

            if (SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting = _masterDataService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id);
            }

            ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (_masterDataService != null)
            {
                if (SessionFacade.TextTranslation == null)
                {
                    SessionFacade.TextTranslation = IsLineManagerApplication()
                        ? _masterDataService.GetTranslationTexts(TranslationType.SelfService).ToList()
                        : _masterDataService.GetTranslationTexts().ToList();
                }

                if (SessionFacade.CurrentUser == null)
                {
                    SessionFacade.CaseTranslation = _masterDataService.GetCaseTranslations();
                }
                else if (SessionFacade.CaseTranslation == null)
                {
                    SessionFacade.CaseTranslation = _masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id);
                }

            }
        }

        //todo: make action filter
        private bool CheckUserAccessToUrl(ActionExecutingContext filterContext)
        {
            var urlSettings = _configurationService.UrlSettings;

            if (urlSettings?.DeniedUrls == null || !urlSettings.DeniedUrls.Any())
                return true;

            foreach (var url in urlSettings.DeniedUrls)
            {
                if (IsStringMatch(url, filterContext.HttpContext.Request.RawUrl))
                {
                    foreach (var allowUrl in urlSettings.AllowedUrls)
                    {
                        if (IsStringMatch(allowUrl, filterContext.HttpContext.Request.RawUrl))
                            return true;
                    }

                    return false;
                }
            }

            return true;
        }

        private bool IsStringMatch(string pattern, string compareStr)
        {
            /*TODO: Use regular experssion!!! */

            var _pattern = pattern.ToLower();
            var _compareStr = compareStr.ToLower();

            var _patternWithoutStar = _pattern.Replace("*", "");
            if (_pattern.EndsWith("*"))
            {
                var purePattern = "";
                var patternWithQuestionMark = "";
                var patternWithSlash = "";
                if (_patternWithoutStar.EndsWith("/"))
                {
                    purePattern = _patternWithoutStar.Remove(_patternWithoutStar.Length - 1);

                    patternWithSlash = $"{purePattern}/";
                    patternWithQuestionMark = $"{purePattern}?";
                    if (_compareStr.StartsWith(patternWithSlash) || _compareStr.StartsWith(patternWithQuestionMark))
                        return true;
                }

                if (_patternWithoutStar.EndsWith("?"))
                {
                    purePattern = _patternWithoutStar.Remove(_patternWithoutStar.Length - 1);
                    patternWithQuestionMark = $"{purePattern}?";
                    if (_compareStr.StartsWith(patternWithQuestionMark))
                        return true;
                }

                if (_compareStr.StartsWith(_patternWithoutStar))
                    return true;
            }


            if (_pattern.EndsWith("{guid}"))
            {
                var _patternWithoutGuid = _patternWithoutStar.Replace("{guid}", "");
                if (_compareStr.Length < _patternWithoutGuid.Length)
                    return false;

                var rightSide = _compareStr.Remove(0, _patternWithoutGuid.Length);
                Guid param;
                try
                {
                    if (Guid.TryParse(rightSide, out param))
                        return true;
                }
                catch
                {
                    return false;
                }
            }

            if (_pattern.EndsWith("{int}"))
            {
                var _patternWithoutInt = _patternWithoutStar.Replace("{int}", "");
                if (_compareStr.Length < _patternWithoutInt.Length)
                    return false;

                var rightSide = _compareStr.Remove(0, _patternWithoutInt.Length);
                int param;
                try
                {
                    if (int.TryParse(rightSide, out param))
                        return true;
                }
                catch
                {
                    return false;
                }
            }

            return _pattern.Equals(_compareStr, StringComparison.CurrentCultureIgnoreCase);
        }

        //keep for diagnostics purposes
        private void LogWithContext(string msg)
        {
            var customerId = SessionFacade.CurrentCustomerID;
            var userIdentityEmail = SessionFacade.CurrentUserIdentity?.Email;
            var userIdentityEmployeeNumber = SessionFacade.CurrentUserIdentity?.EmployeeNumber;
            var userIdentityUserId = SessionFacade.CurrentUserIdentity?.UserId;
            var localUserPkId = SessionFacade.CurrentLocalUser?.Id;
            var localUserId = SessionFacade.CurrentLocalUser?.UserId;

            _log.Debug($@"{msg}. Context: 
                        -customerId: {customerId}, 
                        -userIdentityEmail = {userIdentityEmail},
                        -userIdentityEmployeeNumber = {userIdentityEmployeeNumber},
                        -userIdentityUserId = {userIdentityUserId},
                        -localUserPkId = {localUserPkId},
                        -localUserId = {localUserId}");
        }
    }
}