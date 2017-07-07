namespace DH.Helpdesk.SelfService.Infrastructure
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Linq;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.SelfService.Infrastructure.Extensions;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.Common.Types;
    using System.Security.Claims;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using System.Configuration;    
    using DH.Helpdesk.SelfService.WebServices;
    using DH.Helpdesk.SelfService.WebServices.Common;
    using System.Collections.Generic;
    using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.Extensions.String;


    public class BaseController : Controller
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseSolutionService _caseSolutionService;


        public BaseController(
            IMasterDataService masterDataService,
            ICaseSolutionService caseSolutionService)
        {
            this._masterDataService = masterDataService;
            this._caseSolutionService = caseSolutionService;
        }

        //called before a controller action is executed, that is before ~/HomeController/index 
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var customerId = -1;
            TempData["ShowLanguageSelect"] = true;
            SessionFacade.LastError = null;
            var sessionCustomerId = SessionFacade.CurrentCustomer != null ? SessionFacade.CurrentCustomer.Id : -1;
            customerId = RetrieveCustomer(filterContext, sessionCustomerId);

            if (SessionFacade.CurrentCustomer == null && customerId == -1)
            {
                ErrorGenerator.MakeError("Customer Id can't be empty!", 101);
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                return;
            }

            // CustomerId has been changed by user
            if ((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId && customerId != -1) ||
                (SessionFacade.CurrentCustomer == null && customerId != -1))
            {
                var newCustomer = this._masterDataService.GetCustomer(customerId);
                SessionFacade.CurrentCustomer = newCustomer;
                // Customer changed then clear sessions
                SessionFacade.CurrentCoWorkers = null;
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError(string.Format("Customer is not valid!", customerId), 101);
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                return;
            }
            else
            {
                SessionFacade.UserHasAccess = true;
                customerId = SessionFacade.CurrentCustomer.Id;
                SessionFacade.CurrentCustomerID = customerId;
            }

            if (SessionFacade.AllLanguages == null)
            {
                SessionFacade.AllLanguages = GetActiveLanguages();
            }

            if (filterContext.ActionParameters.Keys.Contains("languageId"))
            {
                var languageIdPassed = filterContext.ActionParameters["languageId"];
                if (!string.IsNullOrEmpty(languageIdPassed.ToString()))
                    SessionFacade.CurrentLanguageId = int.Parse(languageIdPassed.ToString());
            }
            else
            {
                if (SessionFacade.CurrentCustomer != null & SessionFacade.CurrentLanguageId == 0)
                    SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            }

            var appType = ConfigurationManager.AppSettings[AppSettingsKey.CurrentApplicationType].ToString().CleanSpaceAndLowStr();
            var loginMode = ConfigurationManager.AppSettings[AppSettingsKey.LoginMode].ToString().CleanSpaceAndLowStr();

            if (SessionFacade.CurrentUserIdentity == null)
            {
                if (loginMode == LoginMode.SSO)
                {
                    ClaimsPrincipal principal = User as ClaimsPrincipal;

                    if (principal != null)
                    {
                        string claimData = "";
                        bool isFirst = true;
                        var userIdentity = new UserIdentity();

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
                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimDomain].ToString().CleanSpaceAndLowStr())
                                    userIdentity.Domain = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimUserId].ToString().CleanSpaceAndLowStr())
                                    userIdentity.UserId = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimEmployeeNumber].ToString().CleanSpaceAndLowStr())
                                    userIdentity.EmployeeNumber = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimFirstName].ToString().CleanSpaceAndLowStr())
                                    userIdentity.FirstName = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimLastName].ToString().CleanSpaceAndLowStr())
                                    userIdentity.LastName = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimEmail].ToString().CleanSpaceAndLowStr())
                                    userIdentity.Email = value;

                                if (pureType.CleanSpaceAndLowStr() == ConfigurationManager.AppSettings[Enums.FederationServiceKeys.ClaimPhone].CleanSpaceAndLowStr())
                                    userIdentity.Phone = value;
                            }
                        }

                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId]))
                            userIdentity.UserId = ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId].ToString();

                        var netId = principal.Identity.Name;
                        var ssoLog = new NewSSOLog()
                        {
                            ApplicationId = ConfigurationManager.AppSettings[AppSettingsKey.ApplicationId].ToString(),
                            NetworkId = netId,
                            ClaimData = claimData,
                            CreatedDate = DateTime.Now
                        };

                        if (ConfigurationManager.AppSettings[AppSettingsKey.SSOLog].ToString().CleanSpaceAndLowStr() == BooleanString.TRUE &&
                            string.IsNullOrEmpty(SessionFacade.CurrentSystemUser))
                            _masterDataService.SaveSSOLog(ssoLog);

                        if (string.IsNullOrEmpty(userIdentity.UserId))
                        {
                            SessionFacade.UserHasAccess = false;
                            ErrorGenerator.MakeError("You don't have access to the portal. (User Id is not specified)", 107);
                            filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                            return;
                        }
                        else
                        {
                            SessionFacade.CurrentSystemUser = userIdentity.UserId;

                            var defaultEmployeeNumber = ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber].ToString();
                            if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                                userIdentity.EmployeeNumber = defaultEmployeeNumber;

                            SessionFacade.CurrentUserIdentity = userIdentity;
                            SessionFacade.UserHasAccess = true;
                        }
                    }
                } // SSO Login
                else if (loginMode == LoginMode.Windows)
                {
                    var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    SessionFacade.UserHasAccess = true;
                    SessionFacade.CurrentCoWorkers = null;
                    string fullName = identity.Name;
                    string userId = fullName.GetUserFromAdPath();
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId]))
                        userId = ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId].ToString();

                    var employeeNum = string.Empty;
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber]))
                        employeeNum = ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber].ToString();


                    string userDomain = fullName.GetDomainFromAdPath();
                    SessionFacade.CurrentSystemUser = userId;
                    var initiator = _masterDataService.GetInitiatorByUserId(userId, customerId);
                    var ui = new UserIdentity()
                    {
                        UserId = userId,
                        Domain = userDomain,
                        FirstName = initiator?.FirstName,
                        LastName = initiator?.LastName,
                        EmployeeNumber = employeeNum,
                        Phone = initiator?.Phone,
                        Email = initiator?.Email
                    };

                    SessionFacade.CurrentUserIdentity = ui;
                }
                else if (loginMode == LoginMode.Anonymous)
                {
                    var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    SessionFacade.UserHasAccess = true;
                    SessionFacade.CurrentCoWorkers = null;
                    string fullName = identity.Name;
                    string userId = fullName.GetUserFromAdPath();
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId]))
                        userId = ConfigurationManager.AppSettings[AppSettingsKey.DefaultUserId].ToString();

                    var empployNum = "";
                    var defaultEmployeeNumber = ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber].ToString();
                    if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                        empployNum = defaultEmployeeNumber;


                    var employeeNum = string.Empty;
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber]))
                        employeeNum = ConfigurationManager.AppSettings[AppSettingsKey.DefaultEmployeeNumber].ToString();

                    string userDomain = fullName.GetDomainFromAdPath();
                    var initiator = _masterDataService.GetInitiatorByUserId(userId, customerId);
                    SessionFacade.CurrentSystemUser = userId;
                    var ui = new UserIdentity()
                    {
                        UserId = userId,
                        Domain = userDomain,
                        FirstName = initiator?.FirstName,
                        LastName = initiator?.LastName,
                        EmployeeNumber = employeeNum,
                        Phone = initiator?.Phone,
                        Email = initiator?.Email
                    };

                    SessionFacade.CurrentUserIdentity = ui;
                }

                // load user info according database info (tblComputerUser)
                LoadUserInfo();

            } //User Idendity is null

            //load user info from tblUsers if such user exist
            LoadLocalUserInfo();

            if (appType == ApplicationTypes.LineManager)
            {
                if (SessionFacade.CurrentCustomer != null && SessionFacade.CurrentUserIdentity != null && 
                    !string.IsNullOrEmpty(SessionFacade.CurrentUserIdentity.EmployeeNumber))
                {
                    var config = (ECT.FormLib.Configurable.AccessManagment)ConfigurationManager.GetSection("formLibConfigurable/accessManagment");
                    var country = config.Countries.Where(x => x.HelpdeskCustomerId == SessionFacade.CurrentCustomer.Id.ToString()).FirstOrDefault();

                    if (country == null || (country != null && !SessionFacade.CurrentUserIdentity.EmployeeNumber.StartsWith(country.EmployeePrefix)))
                    {
                        SessionFacade.UserHasAccess = false;
                        SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();

                        ErrorGenerator.MakeError("You don't have access to the portal. (User is not manager for country)", 103);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }
                }

                if (SessionFacade.CurrentCoWorkers == null || (SessionFacade.CurrentCoWorkers != null && SessionFacade.CurrentCoWorkers.Count == 0))
                {
                    if (string.IsNullOrEmpty(SessionFacade.CurrentUserIdentity.EmployeeNumber))
                    {
                        SessionFacade.UserHasAccess = false;

                        ErrorGenerator.MakeError("You don't have access to the portal. (Employee Number is not specified)", 104);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return;
                    }
                    else
                    {
                        var _amAPIService = new AMAPIService();
                        var employee = AsyncHelpers.RunSync<APIEmployee>(() => _amAPIService.GetEmployeeFor(SessionFacade.CurrentUserIdentity.EmployeeNumber));


                        if (employee.IsManager)
                        {
                            SessionFacade.CurrentCoWorkers = employee.Subordinates;
                        }
                        else
                        {
                            SessionFacade.UserHasAccess = false;
                            SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();

                            ErrorGenerator.MakeError("You don't have access to the portal. (User is not manager)", 102);
                            filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                            return;
                        }
                    }
                }
            }

            this.SetTextTranslation(filterContext);
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

        private int RetrieveCustomer(ActionExecutingContext filterContext, int sessionCustomerId)
        {
            var ret = -1;

            if (filterContext.ActionParameters.Keys.Contains("customerId", StringComparer.OrdinalIgnoreCase))
            {
                var customerIdPassed = filterContext.ActionParameters["customerId"];                
                if (customerIdPassed != null && !string.IsNullOrEmpty(customerIdPassed.ToString()))
                {
                    int tempId = 0;
                    if (int.TryParse(customerIdPassed.ToString(), out tempId))
                        ret = tempId;
                    else
                    {
                        ErrorGenerator.MakeError("Customer Id not valid!", 105);
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                        return -1;
                    }
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

            if (sessionCustomerId <0 && ret < 0 && filterContext.HttpContext.Request.QueryString["customerId"] != null)
            {
                int paramCustomerId;
                if (int.TryParse(filterContext.HttpContext.Request.QueryString["customerId"], out paramCustomerId))
                    ret = paramCustomerId;
                else
                {
                    ErrorGenerator.MakeError("Customer Id not valid!", 105);
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Error"));
                    return -1;
                }
            }

            return ret;
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