using System.IdentityModel.Services;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Infrastructure;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Linq;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.Common.Types;
    using System.Security.Claims;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using System.Configuration;
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.Extensions.String;
    using BusinessData.Models.Error;
    using System.Web;
    using BusinessData.Models.Employee;
    using Helpers;

    public class BaseController : Controller
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseSolutionService _caseSolutionService;
        private bool userOrCustomerChanged = false; 

        public BaseController(
            IMasterDataService masterDataService,
            ICaseSolutionService caseSolutionService)
        {
            _masterDataService = masterDataService;
            _caseSolutionService = caseSolutionService;
        }

        //called before a controller action is executed, that is before ~/HomeController/index 
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var customerId = -1;
            TempData["ShowLanguageSelect"] = true;
            SessionFacade.LastError = null;
            var lastError = new ErrorModel(string.Empty);
            userOrCustomerChanged = false;

            var appType = AppConfigHelper.GetAppSetting(AppSettingsKey.CurrentApplicationType);
            var loginMode = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
            var isSsoMode = loginMode.Equals(LoginMode.SSO, StringComparison.OrdinalIgnoreCase);
            var federatedAuthenticationSettings = new FederatedAuthenticationSettings();

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

            SetLanguage(filterContext);

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

                else if (loginMode == LoginMode.Windows || loginMode == LoginMode.Anonymous)
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

                userOrCustomerChanged = true;
            } //User Session was null


            if (userOrCustomerChanged)
            {
                SessionFacade.CurrentCoWorkers = null;

                // load user info according database info (tblComputerUser)
                LoadUserInfo();

                //load user info from tblUsers if such user exist
                LoadLocalUserInfo();
            }
            
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
                userOrCustomerChanged = true;
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

        private void SetLanguage(ActionExecutingContext filterContext)
        {
            if (SessionFacade.AllLanguages == null)            
                SessionFacade.AllLanguages = GetActiveLanguages();

            var curUri = filterContext.HttpContext.Request.Url;
            var passedLanguageId = HttpUtility.ParseQueryString(curUri.Query).Get("languageId");

            if (!string.IsNullOrEmpty(passedLanguageId))
            {
                int langId = -1;
                if (int.TryParse(passedLanguageId, out langId))
                    SessionFacade.CurrentLanguageId = langId;
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
            if (SessionFacade.CurrentCustomer != null &&
                SessionFacade.CurrentUserIdentity != null)
            {
                var userNames = SessionFacade.CurrentUserIdentity.UserId.Split(@"\");
                var userNamesToCheck = new List<string>
                {
                    SessionFacade.CurrentUserIdentity.UserId
                };
                if(!string.IsNullOrWhiteSpace(SessionFacade.CurrentUserIdentity.Domain)) userNamesToCheck.Add($@"{SessionFacade.CurrentUserIdentity.Domain}\{SessionFacade.CurrentUserIdentity.UserId}");
                if (userNames.Length > 1) userNamesToCheck.Add(userNames[userNames.Length - 1]);

                foreach (var userName in userNamesToCheck)
                {
                    if(string.IsNullOrWhiteSpace(userName)) continue;

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
                
                var claimDomain = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimDomain);
                var claimUserId = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimUserId);
                var claimEmployeeNumber = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimEmployeeNumber);
                var claimFirstName = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimFirstName);
                var claimLastName = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimLastName);
                var claimEmail = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimEmail);
                var claimPhone = AppConfigHelper.GetAppSetting(Enums.FederationServiceKeys.ClaimPhone);

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

        private UserIdentity TryWindowsLogin(int customerId, out ErrorModel lastError)
        {
            lastError = null;
            UserIdentity userIdentity = null;
            var user = System.Security.Principal.WindowsIdentity.GetCurrent();
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
                var apiCredential = AppConfigHelper.GetAmApiInfo();
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
            if(SessionFacade.AllLanguages != null)
            {
                List<LanguageOverview> allLang = SessionFacade.AllLanguages.ToList();
                var lId = allLang.Where(a => a.LanguageId == language).Select(a => a.Id).SingleOrDefault();
                SessionFacade.CurrentLanguageId = lId;
            }

            currentUrl += "?language=" + language;

            var allParam = lastParams.Split('&');
            foreach(var param in allParam)
            {
                if((!string.IsNullOrEmpty(param)) && (!param.ToLower().Contains("language=")))
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
            var activeLangs = _masterDataService.GetLanguages()
                                        .Where(l => l.IsActive == 1)
                                        .Select(la => new LanguageOverview { Id = la.Id, IsActive = la.IsActive, LanguageId = la.LanguageID, Name = la.Name })
                                        .OrderBy(l => l.Name)
                                        .ToList();
            return activeLangs;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext) //called after a controller action is executed, that is after ~/UserController/index 
        {
            this.SetMasterPageModel(filterContext);
            base.OnActionExecuted(filterContext);

            // Last correct url used for back from Error page
            if(SessionFacade.LastError == null)
                SessionFacade.LastCorrectUrl =
                    filterContext.HttpContext.Request.Url.AbsoluteUri;
        }
       
        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName) : ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);

            if(viewResult == null || (viewResult != null && viewResult.View == null))
                throw new FileNotFoundException("View could not be found");

            this.ViewData.Model = model;
            using(var sw = new StringWriter())
            {
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if(SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer ?? this._masterDataService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = this._masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguageId;
            if(SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = this._masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }
            if(SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting = this._masterDataService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if(this._masterDataService != null)
            {
                if(SessionFacade.TextTranslation == null)
                {
                    if(ConfigurationManager.AppSettings[AppSettingsKey.CurrentApplicationType].ToString().ToLower() == ApplicationTypes.LineManager)
                        SessionFacade.TextTranslation = this._masterDataService.GetTranslationTexts()
                                                                               .Where(x => x.Type == TranslationType.SelfService)
                                                                               .ToList();
                    else
                        SessionFacade.TextTranslation = this._masterDataService.GetTranslationTexts().ToList();
                }

                if(SessionFacade.CurrentUser == null)
                    SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations();
                else
                    if(SessionFacade.CaseTranslation == null)
                        SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id);

            }
        }
    }
    
}