using DH.Helpdesk.Web.Infrastructure.Cache;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DH.Helpdesk.Web.Infrastructure
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Web.UI.WebControls;

    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    //using DH.Helpdesk.Web.Infrastructure.StringExtensions;
    
    using DH.Helpdesk.Web.Models;
    using System.Configuration;
    using System.Security.Claims;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Enums;

    [SessionRequired]
    [CustomAuthorize]
    public class BaseController : Controller
    {
        #region Fields

        private readonly IMasterDataService _masterDataService;

        #endregion

        #region Constructors and Destructors

        public BaseController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        #endregion

        #region Methods

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
            //called after a controller action is executed, that is after ~/UserController/index 
        {
            this.SetMasterPageModel(filterContext);
            this.AutoDetectTimeZoneMessageCheck();
            base.OnActionExecuted(filterContext);
        }

        private void AutoDetectTimeZoneMessageCheck()
        {
            if (!SessionFacade.WasTimeZoneMessageDisplayed)
            {
                SessionFacade.WasTimeZoneMessageDisplayed = true;
                ViewBag.AutoDetectionResult = SessionFacade.TimeZoneDetectionResult;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
            //called before a controller action is executed, that is before ~/UserController/index 
        {
            if (SessionFacade.CurrentUser != null)
            {
                this.SessionCheck(filterContext);
                this.SetTextTranslation(filterContext);

                ApplicationFacade.RemoveCaseUserInfo(SessionFacade.CurrentUser.Id);
                ApplicationFacade.UpdateLoggedInUserActivity(this.Session.SessionID);
            }
            else
            {
                throw new Exception("[DEBUG] BusinessLogicException how it can be?");
            }
        }

        /// <summary>
        /// called when a process requests authorization or authorization occurs before login and before OnActionExecuting + index + OnActionExecuted 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var redirectToUrl = "~/login/login?returnUrl=" + filterContext.HttpContext.Request.Url;            
            var curUserId = "";

            if (string.IsNullOrEmpty(SessionFacade.CurrentLoginMode))
               SessionFacade.CurrentLoginMode = GetCurrentLoginMode();
            

            if (SessionFacade.CurrentUser == null)
            {                                
                switch (SessionFacade.CurrentLoginMode)
                {
                   case LoginMode.Application:
                        curUserId = this.User.Identity.Name;
                        break;
                        
                   case LoginMode.SSO:                        
                        curUserId = GetSSOUserId();                            
                        break;                   
                }

                var user = this._masterDataService.GetUserForLogin(curUserId);
                if (user != null)
                {
                    if (user.TimeZoneId == null)
                    {
                        /// well, if we have AJAX request and user has no TimeZoneId selected in profile,
                        /// set than local time zone (Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna)... siliently
                        user.TimeZoneId = TimeZoneInfo.Local.Id;
                    }

                    SessionFacade.CurrentUser = user;
                    var customerName = this._masterDataService.GetCustomer(user.CustomerId).Name;
                    ApplicationFacade.AddLoggedInUser(
                        new LoggedInUsers
                        {
                            Customer_Id = user.CustomerId,
                            User_Id = user.Id,
                            UserFirstName = user.FirstName,
                            UserLastName = user.SurName,
                            CustomerName = customerName,
                            LoggedOnLastTime = DateTime.UtcNow,
                            LatestActivity = DateTime.UtcNow,
                            SessionId = this.Session.SessionID
                        });
                }
                else
                {
                    filterContext.Result = new RedirectResult(redirectToUrl);
                }
            } // if User Session = Null

            base.OnAuthorization(filterContext);
        }

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial
                ? ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName)
                : ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);

            if (viewResult == null || (viewResult != null && viewResult.View == null))
            {
                throw new FileNotFoundException("View could not be found");
            }

            this.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    this.ControllerContext,
                    viewResult.View,
                    this.ViewData,
                    this.TempData,
                    sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private string GetCurrentLoginMode()
        {                        
            if (ConfigurationManager.AppSettings[AppSettingsKey.LoginMode] == null)
                return LoginMode.Application;
            else
                return ConfigurationManager.AppSettings[AppSettingsKey.LoginMode].ToString().ToLower();                       
        }

        private string GetSSOUserId()
        {
            var userId = "";            
            var adfsSetting = _masterDataService.GetADFSSetting();

            ClaimsPrincipal principal = User as ClaimsPrincipal;            
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
                    if (pureType.Replace(" ", "").ToLower() == adfsSetting.AttrDomain.ToString().Replace(" ", "").ToLower())
                        userIdentity.Domain = value;

                    if (pureType.Replace(" ", "").ToLower() == adfsSetting.AttrUserId.ToString().Replace(" ", "").ToLower())
                        userIdentity.UserId = value;                   
                }
            }                          

            if (adfsSetting.SaveSSOLog)
            {
                var ssoLog = new NewSSOLog()
                {
                    ApplicationId = adfsSetting.ApplicationId,
                    NetworkId = principal.Identity.Name,
                    ClaimData = claimData,
                    CreatedDate = DateTime.Now
                };
                
                _masterDataService.SaveSSOLog(ssoLog);
            }

            if (string.IsNullOrEmpty(userIdentity.UserId))
            {
                this.Session.Clear();
                ApplicationFacade.RemoveLoggedInUser(Session.SessionID);                
            }
            else
            {
                userId = userIdentity.UserId;
                SessionFacade.CurrentUserIdentity = userIdentity;                                                            
            }

            return userId;
            //var redirectToUrl = "" + filterContext.HttpContext.Request.Url;                        
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer
                                                ?? this._masterDataService.GetCustomer(
                                                    SessionFacade.CurrentUser.CustomerId);
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = this._masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguageId;

            masterViewModel.GlobalSettings = this._masterDataService.GetGlobalSettings();

            if (SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = this._masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }
            if (SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting =
                    this._masterDataService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (this._masterDataService != null)
            {
                //_cache.GetTextTranslations();
                //_cache.GetCaseTranslations();
            }
        }

        /// <summary>
        /// Uses Json.net for serialization json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ActionResult JsonDefault(object data, JsonSerializerSettings settings = null)
        {
            return data.ToJsonResult(settings ?? new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        #endregion
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        private string userPermission;

        public string UserPermsissions
        {
            get
            {
                return this.userPermission ?? string.Empty;
            }

            set
            {
                this.userPermission = value;
            }
        }
        
        #region Methods

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Error/Unathorized");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (httpContext.Session == null)
            {
                httpContext.Response.Redirect("~/login/login");
                return false;
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.Roles != string.Empty)
            {
                foreach (string userRole in this.Roles.Split(','))
                {
                    if (GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole))
                    {
                        return true;
                    }
                }

                return false;
            }

            if (this.UserPermsissions != string.Empty)
            {
                foreach (string userPermission in this.UserPermsissions.Split(','))
                {
                    if (GeneralExtensions.UserHasPermission(SessionFacade.CurrentUser, userPermission))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// NO any specific ACL politic is set
            if (this.Roles == string.Empty && this.UserPermsissions == string.Empty)
            {
                return true;
            }
            

            return false;
        }

        #endregion
    }
}