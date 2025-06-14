﻿using System;
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
using Newtonsoft.Json;

namespace DH.Helpdesk.Web.Controllers
{
    public class LoginController : Controller
    {
        private const string Root = "/";
        
        //private const string TokenKey = "Token_Data";
        //private const string Access_Token_Key = "Access_Token";
        //private const string Refresh_Token_Key = "Refresh_Token";
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
            var appconfig = new ApplicationConfiguration();
            if (_applicationConfiguration.LoginMode == LoginMode.SSO)
            {
                var loginUrl = _federatedAuthenticationService.GetSignInUrl();
                return Redirect(loginUrl);
            }
            var msLogin = appconfig.GetAppKeyValueMicrosoft;
            ViewBag.ShowMsButton = false;
            if (msLogin == "1")
            {
                ViewBag.ShowMsButton = true;
            }

            ViewBag.ReCaptchaSiteKey = appconfig.GetRecaptchaSiteKey;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginInputModel inputData)
        {
            var logger = Infrastructure.Logger.LogManager.reCaptcha;
            //TODO: Logga inputData.reCaptchaToken
            logger.Info($"Login: inputData.reCaptchaToken: {inputData.reCaptchaToken}");

            var userName = inputData.txtUid?.Trim();
            var password = inputData.txtPwd?.Trim();
            var returnUrl = string.IsNullOrEmpty(inputData.returnUrl) ? "~/" : inputData.returnUrl;
            var reCaptchaToken = inputData.reCaptchaToken;
            var appconfig = new ApplicationConfiguration();
            var siteKey = appconfig.GetRecaptchaSiteKey;

            //TODO: Logga sitekey
            logger.Info($"Login: siteKey: {siteKey}");
            if (!string.IsNullOrEmpty(siteKey) && string.IsNullOrEmpty(inputData.reCaptchaToken))
            {
                //TODO: Logga att vi kom in här
                logger.Info($"Login: inputData.reCaptchaToken is empty");
                TempData["LoginFailed"] = "Login failed! Couldn't verify you with reCaptcha".Trim();
                ViewBag.ReCaptchaSiteKey = appconfig.GetRecaptchaSiteKey;
                return View("Login");
            }

            // Verify reCaptcha token if required
            if (!String.IsNullOrEmpty(reCaptchaToken) && !VerifyRecaptcha(reCaptchaToken))
            {
                //TODO: Logga att vi kom in här
                logger.Info($"Login: VerifyRecaptcha failed");
                TempData["LoginFailed"] = "Login failed! Couldn't verify you with reCaptcha".Trim();
                ViewBag.ReCaptchaSiteKey = appconfig.GetRecaptchaSiteKey;
                return View("Login");
            }

            //TODO: Logga att allting OK
            logger.Info($"Login with reCaptcha: All OK");
            // Validate login arguments
            if (IsValidLoginArgument(userName, password))
            {
                var res = _authenticationService.Login(
                    HttpContext,
                    userName,
                    password,
                    new UserTimeZoneInfo(inputData.timeZoneOffsetInJan1, inputData.timeZoneOffsetInJul1)
                );

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

            // Set ViewBag properties
            ViewBag.ShowMsButton = appconfig.GetAppKeyValueMicrosoft == "1";
            ViewBag.ReCaptchaSiteKey = appconfig.GetRecaptchaSiteKey;
            return View("Login");
        }

        public bool VerifyRecaptcha(string token)
        {
            var appconfig = new ApplicationConfiguration();
            var secret = appconfig.GetRecaptchaSecretKey;

            //TODO: Logga secret
            var logger = Infrastructure.Logger.LogManager.reCaptcha;
            logger.Info($"VerifyRecaptcha: secret: {secret}");
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "secret", secret },
                    { "response", token }
                };

                var content = new FormUrlEncodedContent(values);
                var recaptchaEndPoint = appconfig.GetRecaptchaEndPoint;
                var response =client.PostAsync(recaptchaEndPoint, content);
                var responseString = response.Result.Content.ReadAsStringAsync().Result;
                var recaptchaMinScore = appconfig.GetRecaptchaMinScore;

                //TODO: Logga responseString && recaptchaMinScore
                logger.Info($"VerifyRecaptcha: responseString: {responseString}");
                logger.Info($"VerifyRecaptcha: recaptchaMinScore: {recaptchaMinScore}");
                //Deserialize the incoming object
                var responseJson = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);
                if(responseJson.Success && responseJson.Score > recaptchaMinScore)
                {
                    //TODO: Logga att vi kom in här
                    logger.Info($"VerifyRecaptcha: Success && Score > recaptchaMinScore");
                    return true;
                }
                else
                {
                    //TODO: Logga att vi kom in här
                    logger.Info($"VerifyRecaptcha: Failed");
                    return false;
                }
            }
        }

        [AllowAnonymous]
        public void SignIn(LoginInputModel inputData)
        {
            var returnUrlMS = inputData.returnUrlMS;
            if(!string.IsNullOrEmpty(returnUrlMS))
            {
                returnUrlMS = returnUrlMS.Replace("~/", "");
            }
            else
            {
                returnUrlMS = "/";
            }
            _authenticationService.SetLoginModeToMicrosoft();
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = returnUrlMS },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);

        }

        [HttpGet]
        public ActionResult Logout()
        {
            _authenticationService.ClearLoginSession(ControllerContext.HttpContext);
            var appconfig = new ApplicationConfiguration();
            var msLogin = appconfig.GetAppKeyValueMicrosoft;
            ViewBag.ShowMsButton = false;
            if (msLogin == "1")
            {
                ViewBag.ShowMsButton = true;
            }
            ViewBag.ReCaptchaSiteKey = appconfig.GetRecaptchaSiteKey;
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

        #endregion
    }
    class RecaptchaResponse
    {

        public DateTime challenge_ts { get; set; }
        public string Hostname { get; set; }
        public bool Success { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
    }
}
