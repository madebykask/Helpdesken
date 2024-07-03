using System;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Users;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using DH.Helpdesk.Web.Infrastructure.Tools;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Models.Login;
using DH.Helpdesk.Web.Models.Shared;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DH.Helpdesk.Web.Controllers
{
    public class LoginController : Controller
    {
        private const string Root = "/";
        //private const string TokenKey = "Token_Data";
        //private const string Access_Token_Key = "Access_Token";
        //private const string Refresh_Token_Key = "Refresh_Token";
        private ApplicationConfiguration appconfig;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IUsersPasswordHistoryService _usersPasswordHistoryService;
        private readonly ICaseLockService _caseLockService;
        private readonly IRouteResolver _routeResolver;

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IFederatedAuthenticationService _federatedAuthenticationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationServiceBehaviorFactory _behaviorFactory;
        public IAuthenticationBehavior _authenticationBehavior;
        private readonly IGlobalSettingService _globalSettingService;
        public LoginController(
                IUserService userService, 
                ICustomerService customerService, 
                ISettingService settingService, 
                IUsersPasswordHistoryService usersPasswordHistoryService,
                ICaseLockService caseLockService,
                ILanguageService languageService, 
                IRouteResolver routeResolver,
                ILogProgramService logProgramService,
                IApplicationConfiguration applicationConfiguration,
                IFederatedAuthenticationService federatedAuthenticationService,
                IAuthenticationService authenticationService,
                IAuthenticationServiceBehaviorFactory behaviorFactory,
                            IGlobalSettingService globalSettingService)
        {
            _userService = userService;
            _settingService = settingService;
            _usersPasswordHistoryService = usersPasswordHistoryService;
            _caseLockService = caseLockService;
            _routeResolver = routeResolver;
            _applicationConfiguration = applicationConfiguration;
            _federatedAuthenticationService = federatedAuthenticationService;
            _authenticationService = authenticationService;
            _behaviorFactory = behaviorFactory;
            _globalSettingService = globalSettingService;
        }
     
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [AllowAnonymous]
        public ActionResult Login()
        {

            if (_applicationConfiguration.LoginMode == LoginMode.SSO)
            {
                var loginUrl = _federatedAuthenticationService.GetSignInUrl();
                return Redirect(loginUrl);
            }
            appconfig = new ApplicationConfiguration();
            var msLogin = appconfig.GetAppKeyValueMicrosoft;
            ViewBag.ShowMsButton = false;
            if (msLogin == "1")
            {
                ViewBag.ShowMsButton = true;
            }
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public  ActionResult Login(LoginInputModel inputData) 
        {
            var userName = inputData.txtUid?.Trim();
            var password = inputData.txtPwd?.Trim();
            var returnUrl = inputData.returnUrl;
            var reCaptchaToken = inputData.reCaptchaToken;
            var verified = VerifyRecaptcha(reCaptchaToken);
            if(string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }

            if (IsValidLoginArgument(userName, password))
            {
                // try to login user
                var res = 
                    _authenticationService.Login(
                        HttpContext, 
                        userName, 
                        password,
                        new UserTimeZoneInfo(inputData.timeZoneOffsetInJan1, inputData.timeZoneOffsetInJul1));

                if (res.IsSuccess)
                {
                    SessionFacade.TimeZoneDetectionResult = res.TimeZoneAutodetect;

                    if (res.PasswordExpired)
                    {
                        var settings = _settingService.GetCustomerSetting(res.User.CustomerId);

                        ViewBag.UserId = userName;
                        ViewBag.ChangePasswordModel = GetPasswordChangeModel(res.User, settings);
                        return View("Login");
                    }

                    _caseLockService.CaseLockCleanUp();

                    RedirectFromLoginPage(returnUrl, res.User.StartPage, res.TimeZoneAutodetect);
                }
                else
                {
                    TempData["LoginFailed"] = $"Login failed! {res.ErrorMessage ?? string.Empty}".Trim();
                    
                }
            }

            appconfig = new ApplicationConfiguration();
            var msLogin = appconfig.GetAppKeyValueMicrosoft;
            ViewBag.ShowMsButton = false;
            if (msLogin == "1")
            {
                ViewBag.ShowMsButton = true;
            }

            return View("Login");


        }
        public bool VerifyRecaptcha(string token)
        {
            appconfig = new ApplicationConfiguration();
            var secret = appconfig.GetRecaptchaSecretKey;
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "secret", secret },
                    { "response", token }
                };

                var content = new FormUrlEncodedContent(values);

                var response =client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var responseString = response.Result.Content.ReadAsStringAsync().Result;

                var responseJson = JObject.Parse(responseString);
                var success = responseJson["success"].Value<bool>();

                return success;
            }
        }

        [AllowAnonymous]
        public void SignIn(LoginInputModel inputData)
        {
            var returnUrl = inputData.returnUrl;
            if(!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = returnUrl.Replace("~/", "");
            }
            else
            {
                returnUrl = "/";
            }
            _authenticationService.SetLoginModeToMicrosoft();
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = returnUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);

        }

        [HttpGet]
        public ActionResult Logout()
        {
            _authenticationService.ClearLoginSession(ControllerContext.HttpContext);
            appconfig = new ApplicationConfiguration();
            var msLogin = appconfig.GetAppKeyValueMicrosoft;
            ViewBag.ShowMsButton = false;
            if (msLogin == "1")
            {
                ViewBag.ShowMsButton = true;
            }
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetUserCount(string userId, string pass)
        {
            var model = new UserStatisticsModel(string.Empty);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pass))
                return View(model);

            if (_userService.IsUserValidAdmin(userId, pass))
            {
                var nums = 0;
                if (ApplicationFacade.LoggedInUsers != null)
                    nums = ApplicationFacade.LoggedInUsers.Count();

                model = new UserStatisticsModel(nums.ToString());
                return View(model);
            }
            else
            {                
                return View(model);
            }
        }

        #region Private Methods

        private bool IsValidLoginArgument(string userName, string password)
        {
            return !(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password));
        }
        
        private void RedirectFromLoginPage(string returnUrl, int startPage, TimeZoneAutodetectResult timeZoneAutodetectResult)
        {
            var redirectTo = string.Empty;

            if (timeZoneAutodetectResult == TimeZoneAutodetectResult.Failure)
            {
                redirectTo = Url.Action("Edit", "Profile", null, Request.Url?.Scheme ?? "https");
            }
            else
            {
                if(returnUrl != "~/")
                {
                    redirectTo = Server.UrlDecode(returnUrl);
                }
                else
                {
                    redirectTo = "";
                }
                //if (!string.IsNullOrEmpty(returnUrl) &&
                //    HttpContext.Request.IsAbsoluteUrlLocalToHost(returnUrl) &&
                //    _routeResolver.AbsolutePathToRelative(returnUrl) != Root)
                //{
                //    redirectTo = Server.UrlDecode(returnUrl);
                //}

                //if (!string.IsNullOrEmpty(redirectTo) && redirectTo.ToLower().Contains("login"))
                //{
                //    redirectTo = Root;
                //}
            }

            Response.Redirect(!string.IsNullOrEmpty(redirectTo) ? redirectTo : _routeResolver.ResolveStartPage(Url, startPage));
        }

        private ChangePasswordModel GetPasswordChangeModel(UserOverview user, Setting settings)
        {
            return new ChangePasswordModel()
            {
                UserId = user.Id,
                MinPasswordLength = settings.MinPasswordLength > 0 ? settings.MinPasswordLength : 5,
                UseComplexPassword = settings.ComplexPassword.ToBool()
            };
        }

        private int GetLiveUserCount()
        {
            if (ApplicationFacade.LoggedInUsers != null)
                return ApplicationFacade.LoggedInUsers.Count();
            else
                return 0;
        }

        private UserIdentity CreateUserIdentity(UserOverview user)
        {
            return new UserIdentity(user.UserId)
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.SurName
            };
        }

        #region Token based support

        //private SimpleToken GetToken(string userName, string password)
        //{
        //    var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'));
        //    var webApiService = new WebApiService(baseUrl);
        //    var token = AsyncHelper.RunSync(() => webApiService.GetAccessToken(userName, password));
        //
        //    if (token != null)
        //    {
        //        var encriptionKey = ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingsKey.EncryptionKey) ?
        //            ConfigurationManager.AppSettings[AppSettingsKey.EncryptionKey].ToString() : string.Empty;
        //                    
        //        token.access_token = AESCryptoProvider.Encrypt256(token.access_token, encriptionKey);
        //        token.refresh_token = AESCryptoProvider.Encrypt256(token.refresh_token, encriptionKey);                
        //    }
        //
        //    return token;
        //}

        //private Dictionary<string,string> GetTokenData(string access_token, string refresh_token)
        //{
        //    var tempData = new Dictionary<string, string>();
        //    tempData.Add(Access_Token_Key, access_token);
        //    tempData.Add(Refresh_Token_Key, refresh_token);
        //    return tempData;
        //}

        #endregion

        #endregion
    }
}
