namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.Services.Infrastructure.TimeZoneResolver;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Users;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Authentication;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.BusinessData.Models.LogProgram;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
    using Models.Login;
    using System.Web.Hosting;
    using Infrastructure.WebApi;
    using Infrastructure.Helpers;
    using Models.WebApi;
    using Infrastructure.Cryptography;
    using System.Configuration;
    using System.Collections.Generic;

    public class LoginController : Controller
    {
        private const string Root = "/";
        private const string TokenKey = "Token_Data";
        private const string Access_Token_Key = "Access_Token";
        private const string Refresh_Token_Key = "Refresh_Token";

        private readonly IUserService userService;
        private readonly ICustomerService customerService;
        private readonly ILanguageService languageService;
        private readonly IUsersPasswordHistoryService usersPasswordHistoryService;
        private readonly ICaseLockService _caseLockService;

        private readonly IRouteResolver routeResolver;

        private readonly ILogProgramService _logProgramService;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(
                IUserService userService, 
                ICustomerService customerService, 
                IUsersPasswordHistoryService usersPasswordHistoryService,
                ICaseLockService caseLockService,
                ILanguageService languageService, 
                IRouteResolver routeResolver,
                ILogProgramService logProgramService,
                IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.customerService = customerService;
            this.usersPasswordHistoryService = usersPasswordHistoryService;
            this._caseLockService = caseLockService;
            this.languageService = languageService;
            this.routeResolver = routeResolver;
            this._logProgramService = logProgramService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _authenticationService.SignOut(ControllerContext.HttpContext);

            TempData[TokenKey] = GetTokenData(string.Empty, string.Empty);

            return this.View("Login");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        public ViewResult Login()
        {
            TempData[TokenKey] = GetTokenData(string.Empty, string.Empty);            
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection coll, string returnUrl)
        {
           
            string userName = coll["txtUid"].Trim();
            string password = coll["txtPwd"].Trim();


            if (this.IsValidLoginArgument(userName, password))
            {
                var user = this.userService.Login(userName, password);

                if (user != null)
                {
                    var redirectTo = string.Empty;
                    if (!string.IsNullOrEmpty(returnUrl) 
                        && HttpContext.Request.IsAbsoluteUrlLocalToHost(returnUrl)
                        && this.routeResolver.AbsolutePathToRelative(returnUrl) != Root)
                    {
                        redirectTo = Server.UrlDecode(returnUrl);
                    }

                    if (!string.IsNullOrEmpty(redirectTo) 
                        && redirectTo.ToLower().Contains("login"))
                    {
                        redirectTo = Root;
                    }

                    this.Session.Clear();
                    int timeZoneOffsetInJan1, timeZoneOffsetInJul1;
                    if (int.TryParse(coll["timeZoneOffsetInJan1"], out timeZoneOffsetInJan1)
                        && int.TryParse(coll["timeZoneOffsetInJul1"], out timeZoneOffsetInJul1))
                    {
                        TimeZoneInfo[] tzones;
                        SessionFacade.TimeZoneDetectionResult = TimeZoneResolver.DetectTimeZone(timeZoneOffsetInJan1, timeZoneOffsetInJul1, out tzones);
                        if (string.IsNullOrEmpty(user.TimeZoneId))
                        {
                            if (SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.Failure)
                            {
                                user.TimeZoneId = TimeZoneInfo.Local.Id;
                            }
                            else
                            {
                                user.TimeZoneId = tzones[0].Id;
                                
                                // we dont want to show any messages to user if we successfully detect his timezone
                                SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.None;
                            }
                        }
                        else
                        {
                            if (SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.Failure)
                            {
                                user.TimeZoneId = TimeZoneInfo.Local.Id;
                            }
                            else
                            {
                                if (tzones.All(it => it.Id != user.TimeZoneId))
                                {
                                    /// notice to user about to change his time zone in profile
                                    SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.Notice;
                                }
                                else
                                {
                                    /// no changes in users time zone was made
                                    SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.None;
                                }
                            }
                        }
                    }
                    else
                    {
                       user.TimeZoneId = TimeZoneInfo.Local.Id;
                       SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.Failure;
                    }

                    SessionFacade.CurrentUser = user;
                    SessionFacade.CurrentLanguageId = user.LanguageId;
                    SessionFacade.CurrentLoginMode = LoginMode.Application;

                    var language = this.languageService.GetLanguage(user.LanguageId);

                    if (language != null) 
                    {
                        SessionFacade.CurrentLanguageCode = language.LanguageID;
                    }

                    var customer = this.customerService.GetCustomer(user.CustomerId);
                    ApplicationFacade.AddLoggedInUser(
                        new LoggedInUsers
                            {
                                Customer_Id = user.CustomerId,
                                User_Id = user.Id,
                                UserFirstName = user.FirstName,
                                UserLastName = user.SurName,
                                CustomerName = customer.Name,
                                LoggedOnLastTime = DateTime.UtcNow,
                                SessionId = Session.SessionID
                            });

                    var logProgramModel = new LogProgram()
                    {
                        CaseId = 0,
                        CustomerId = user.CustomerId,
                        LogType = 2,  //ToDo: define in Enum
                        LogText = this.Request.GetIpAddress(),
                        New_Performer_user_Id = 0,
                        Old_Performer_User_Id = "0",
                        RegTime = DateTime.UtcNow,
                        UserId = user.Id,
                        ServerNameIP = string.Format("{0} ({1})", Environment.MachineName, Request.ServerVariables["LOCAL_ADDR"]),
                        NumberOfUsers = GetLiveUserCount()                        
                    };

                    _logProgramService.UpdateUserLogin(logProgramModel);

                    this.usersPasswordHistoryService.SaveHistory(user.Id, EncryptionHelper.GetMd5Hash(password));
                    this._caseLockService.CaseLockCleanUp();

                    if (SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.Failure)
                    {
                        this.RedirectFromLoginPage(userName, "~/Profile/Edit/", user.StartPage);
                        return null;
                    }

                    //var token = GetToken(userName, password);
                    //TempData[TokenKey] = GetTokenData(string.Empty, string.Empty);
                    
                    //if (token != null)
                    //{
                    //    TempData[TokenKey] = GetTokenData(token.access_token, token.refresh_token);                        
                    //}

                    this.RedirectFromLoginPage(userName, redirectTo, user.StartPage);
                }
                else
                {
                    this.TempData["LoginFailed"] = "Login failed! The user name or password entered is incorrect.";
                }
            }

            return this.View("Login");
        }

        
        private int GetLiveUserCount()
        {
            if (ApplicationFacade.LoggedInUsers != null)
                return ApplicationFacade.LoggedInUsers.Count();
            else
                return 0;                
        }

        [HttpGet]
        public ActionResult GetUserCount(string userId, string pass)
        {
            var model = new UserStatisticsModel(string.Empty);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pass))
                return View(model);

            if (userService.IsUserValidAdmin(userId, pass))
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

        [NonAction]
        private bool IsValidLoginArgument(string userName, string password)
        {
            return !(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password));
        }

        [NonAction]
        private void RedirectFromLoginPage(string userName, string returnUrl, int startPage)
        {
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(10), false, SessionFacade.CurrentUser.ToString());

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            this.Response.Cookies.Add(cookie);

            this.Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : this.routeResolver.ResolveStartPage(this.Url, startPage));
        }

        private SimpleToken GetToken(string userName, string password)
        {
            var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'));
            var webApiService = new WebApiService(baseUrl);
            var token = AsyncHelper.RunSync(() => webApiService.GetAccessToken(userName, password));

            if (token != null)
            {
                var encriptionKey = ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingsKey.EncryptionKey) ?
                                    ConfigurationManager.AppSettings[AppSettingsKey.EncryptionKey].ToString() : string.Empty;
                            
                token.access_token = AESCryptoProvider.Encrypt256(token.access_token, encriptionKey);
                token.refresh_token = AESCryptoProvider.Encrypt256(token.refresh_token, encriptionKey);                
            }

            return token;
        }

        private Dictionary<string,string> GetTokenData(string access_token, string refresh_token)
        {
            var tempData = new Dictionary<string, string>();
            tempData.Add(Access_Token_Key, access_token);
            tempData.Add(Refresh_Token_Key, refresh_token);
            return tempData;
        }
    }
}
