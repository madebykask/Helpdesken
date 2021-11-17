using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Infrastructure.TimeZoneResolver;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.Users;
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.Tools;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public interface IAuthenticationService
    {
        LoginResult Login(HttpContextBase ctx, string userName, string pwd, UserTimeZoneInfo userTimeZoneInfo);
        bool SignIn(HttpContextBase ctx);
        bool SignInApplicationUser(HttpContextBase ctx, int userId);
        void ClearLoginSession(HttpContextBase ctx);
        HttpCookie CreateFormsAuthCookie(string userName, string userData);
        string GetSiteLoginPageUrl();
        string GetAuthenticationModeLoginUrl();
        LoginMode SetLoginModeToMicrosoft();
        LoginMode SetLoginModeToApplication();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApplicationConfiguration _appConfiguration;
        private readonly IAdfsConfiguration _adfsConfiguration;
        private readonly IUserService _userService;
        private readonly ILogProgramService _logProgramService;
        private readonly ISettingService _settingService;
        private readonly ILanguageService _languageService;
        private readonly IUsersPasswordHistoryService _usersPasswordHistoryService;
        private readonly IApplicationContext _applicationContext;
        private readonly IUserContext _userContext;
        private readonly ICustomerContext _customerContext;
        private readonly ISessionContext _sessionContext;
        public IAuthenticationBehavior _authenticationBehavior;
        private readonly ILoggerService _logger = LogManager.Session;
        private readonly IAuthenticationServiceBehaviorFactory _behaviorFactory;

        #region ctor()

        public AuthenticationService(IApplicationConfiguration appConfiguration, 
            IAdfsConfiguration adfsConfiguration,
            IAuthenticationServiceBehaviorFactory behaviorFactory,
            ILogProgramService logProgramService,
            IUserService userService,
            ILanguageService languageService,
            ISettingService settingService,
            IUsersPasswordHistoryService usersPasswordHistoryService,
            IApplicationContext applicationContext,
            ISessionContext sessionContext,
            IUserContext userContext,
            ICustomerContext customerContext)
        {
            _appConfiguration = appConfiguration;
            _adfsConfiguration = adfsConfiguration;
            _userService = userService;
            _logProgramService = logProgramService;
            _languageService = languageService;
            _usersPasswordHistoryService = usersPasswordHistoryService;
            _settingService = settingService;
            _behaviorFactory = behaviorFactory;

            _customerContext = customerContext;
            _userContext = userContext;
            _applicationContext = applicationContext;
            _sessionContext = sessionContext;

            _authenticationBehavior = behaviorFactory.Create(appConfiguration.LoginMode);
        }

        #endregion
        public LoginMode SetLoginModeToMicrosoft()
        {
            _sessionContext.SetLoginMode(LoginMode.Microsoft);
            _authenticationBehavior = _behaviorFactory.Create(LoginMode.Microsoft);
            return _sessionContext.LoginMode;
            
        }
        public LoginMode SetLoginModeToApplication()
        {
            _sessionContext.SetLoginMode(LoginMode.Application);
            _authenticationBehavior = _behaviorFactory.Create(LoginMode.Application);
            return _sessionContext.LoginMode;

        }
        public LoginResult Login(HttpContextBase ctx, string userName, string pwd, UserTimeZoneInfo userTimeZoneInfo)
        {
            var user = _userService.Login(userName, pwd);

            if (user == null)
            {
                return LoginResult.Failed("The user name or password entered is incorrect.");
            }

            var loginResult = LoginResult.Success(user);

            //set user preferred language for UI translation
            _sessionContext.SetCurrentLanguageId(user.LanguageId);
            
            #region Password Expire check

            var passwordChangedDate = _userService.GetUserPasswordChangedDate(user.Id);
            var settings = _settingService.GetCustomerSetting(user.CustomerId);

            if (settings.MaxPasswordAge > 0 && DateTime.Now > passwordChangedDate.AddDays(settings.MaxPasswordAge))
            {
                loginResult.PasswordExpired = true;
                return loginResult;
            }

            #endregion

            ctx.Session.Clear();

            var userIdentity = new UserIdentity(user.UserId)
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.SurName,
                Phone = user.Phone,
                //EmployeeNumber = ?
                //Domain = ?
            };

            // override with test values from config if required
            ApplyUserIdentityOverrides(userIdentity);

            //set app and session state
            _sessionContext.ClearSession();
            _sessionContext.SetUserIdentity(userIdentity);
            _userContext.SetCurrentUser(user);
            _customerContext.SetCustomer(user.CustomerId);
            
            //set user preferred language for UI translation
            _sessionContext.SetCurrentLanguageId(user.LanguageId);

            var language = _languageService.GetLanguage(user.LanguageId);
            if (language != null)
                _sessionContext.SetCurrentLanguageCode(language.LanguageID);

            //try to auto detect user time zone
            loginResult.TimeZoneAutodetect = 
                SetUserTimeZone(userTimeZoneInfo.TimeZoneOffsetInJan1, userTimeZoneInfo.TimeZoneOffsetInJul1, user);

            //save logged in user information
            AddLoggedInUser(user, _customerContext, ctx);
            UpdateUserLogin(ctx, user);

            _usersPasswordHistoryService.SaveHistory(user.Id, EncryptionHelper.GetMd5Hash(pwd));

            //Create FormsAuth cookie for forms Or mixed authentication modes
            if (_appConfiguration.LoginMode == LoginMode.Application || _appConfiguration.LoginMode == LoginMode.Mixed)
            {
                var cookie = CreateFormsAuthCookie(user.UserId, user.ToString()); //todo: check if userData is correct?
                ctx.Response.Cookies.Add(cookie);
            }

            return loginResult;
        }

        public bool SignIn(HttpContextBase ctx)
        {
            _logger.Debug($"AuthenticationService.SignIn: authenticating user. LoginMode: {_sessionContext.LoginMode}");

            var userIdentity = _authenticationBehavior.CreateUserIdentity(ctx);

            if (!string.IsNullOrWhiteSpace(userIdentity?.UserId))
            {
                ApplyUserIdentityOverrides(userIdentity);
                _sessionContext.SetUserIdentity(userIdentity);

                _logger.Debug("AuthenticationService.SignIn: user has successfully been authenticated. " +
                             $"User: {userIdentity.UserId}, Domain: {userIdentity.Domain}, FullName: {userIdentity.FirstName + " " + userIdentity.LastName} ");

                //get customer user (tblUsers)
                var customerUser = GetLocalUser(userIdentity);

                // Todo - Create cookie?
                
                //set only if its non-empty
                if (customerUser != null)
                {
                    //set user and customer context
                    _userContext.SetCurrentUser(customerUser);
                    _customerContext.SetCustomer(customerUser.CustomerId);

                    FixUserTimeZone(customerUser);

                    AddLoggedInUser(customerUser, _customerContext, ctx);
                    UpdateUserLogin(ctx, customerUser);
                    //_caseLockService.CaseLockCleanUp(); //todo: check if required?
                }
                else
                {
                    _logger.Warn($"AuthenticationService.SignIn: Customer user doesn't exist for login '{userIdentity.UserId}'.");
                    return false;
                }
            }
            else
            {
                _logger.Warn($"AuthenticationService.SignIn: User identity is null or empty.");
                return false;
            }
            

            return true;
        }
        public bool SignInApplicationUser(HttpContextBase ctx, int userId)
        {
            _logger.Debug($"AuthenticationService.SignIn: authenticating user. LoginMode: {_sessionContext.LoginMode}");

            var userIdentity = _authenticationBehavior.CreateUserIdentityById(userId);

            if (!string.IsNullOrWhiteSpace(userIdentity?.UserId))
            {
                //    ApplyUserIdentityOverrides(userIdentity);
                //    _sessionContext.SetUserIdentity(userIdentity);

                //    _logger.Debug("AuthenticationService.SignIn: user has successfully been authenticated. " +
                //                 $"User: {userIdentity.UserId}, Domain: {userIdentity.Domain}, FullName: {userIdentity.FirstName + " " + userIdentity.LastName} ");

                //    //get customer user (tblUsers)
                //    var customerUser = GetLocalUser(userIdentity);

                //    //set only if its non-empty
                //    if (customerUser != null)
                //    {
                //        //set user and customer context
                //        _userContext.SetCurrentUser(customerUser);
                //        _customerContext.SetCustomer(customerUser.CustomerId);

                //        FixUserTimeZone(customerUser);

                //        AddLoggedInUser(customerUser, _customerContext, ctx);
                //        UpdateUserLogin(ctx, customerUser);
                //        //_caseLockService.CaseLockCleanUp(); //todo: check if required?
                //        _sessionContext.ClearSession();
                //        _sessionContext.SetUserIdentity(userIdentity);

                //    }
                //    else
                //    {
                //        _logger.Warn($"AuthenticationService.SignIn: Customer user doesn't exist for login '{userIdentity.UserId}'.");
                //        return false;
                //    }
                //}
                //else
                //{
                //    _logger.Warn($"AuthenticationService.SignIn: User identity is null or empty.");
                //    return false;
                //}


                return true;
            }
            return false;
        }

        // try to load users by different formats
        private UserOverview GetLocalUser(UserIdentity userIdentity)
        {
            UserOverview res = null;

            var userId = userIdentity.UserId;
            var userNames = new List<string>
            {
                userId
            };

            var userIdWithoutDomain = GetUserNameWithoutDomain(userId);
            var domain = userIdentity.Domain;
            if (!string.IsNullOrWhiteSpace(domain))
            {
                var userIdWithDomain = $@"{domain}\{userIdWithoutDomain}";
                userNames.Add(userIdWithDomain);
            }

            userNames.Add(userIdWithoutDomain);

            foreach (var userName in userNames)
            {
                res = _userService.GetUserByLogin(userName, null);
                if (res != null)
                    break;
            }

            return res;
        }

        public void ClearLoginSession(HttpContextBase ctx)
        {
           _logger.Debug("AuthenticationService. Clearing logging session.");

            var sessionId = _sessionContext.SessionId;
            var loginMode = _sessionContext.LoginMode;
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                _applicationContext.RemoveLoggedInUser(ctx.Session.SessionID);
                _sessionContext.ClearSession();
            }
            //Clear Microsoft login which is not set in config file
            if (loginMode == LoginMode.Microsoft)
            {
                ctx.GetOwinContext().Authentication.SignOut();
            }
            //clear auth cookie for forms/mixed modes
            if (_appConfiguration.LoginMode == LoginMode.Application ||
                _appConfiguration.LoginMode == LoginMode.Mixed
)
            {
                FormsAuthentication.SignOut();
            }

        }

        public string GetSiteLoginPageUrl()
        {
            return FormsAuthentication.LoginUrl ?? "/Login/Login";
        }

        public string GetAuthenticationModeLoginUrl()
        {
            return _authenticationBehavior.GetLoginUrl();
        }

        public HttpCookie CreateFormsAuthCookie(string userName, string userData)
        {
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(10), false, userData);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            return cookie;
        }

        #region Helper Methods

        private void ApplyUserIdentityOverrides(UserIdentity userIdentity)
        {
            if (_appConfiguration.LoginMode == LoginMode.SSO)
            {
                //override userId with value from config
                if (!string.IsNullOrWhiteSpace(_adfsConfiguration.DefaultUserId))
                    userIdentity.UserId = _adfsConfiguration.DefaultUserId;

                //override employeeNumber with value from config
                if (!string.IsNullOrWhiteSpace(_adfsConfiguration.DefaultEmployeeNumber))
                    userIdentity.EmployeeNumber = _adfsConfiguration.DefaultEmployeeNumber;
            }
        }

        private void AddLoggedInUser(UserOverview user, ICustomerContext customerCtx, HttpContextBase ctx)
        {
            var loggedInUserData = new LoggedInUsers
            {
                Customer_Id = user.CustomerId,
                User_Id = user.Id,
                UserFirstName = user.FirstName,
                UserLastName = user.SurName,
                CustomerName = customerCtx?.CustomerName,
                LoggedOnLastTime = DateTime.UtcNow,
                LatestActivity = DateTime.UtcNow,
                SessionId = ctx.Session.SessionID
            };

            _applicationContext.AddLoggedInUser(loggedInUserData);
        }

        private void UpdateUserLogin(HttpContextBase ctx, UserOverview customerUser)
        {
            var logProgramModel = new LogProgram()
            {
                CaseId = 0,
                CustomerId = customerUser.CustomerId,
                LogType = 2, //ToDo: define in Enum
                LogText = ctx.Request.GetIpAddress(),
                New_Performer_user_Id = 0,
                Old_Performer_User_Id = "0",
                RegTime = DateTime.UtcNow,
                UserId = customerUser.Id,
                ServerNameIP = $"{Environment.MachineName} ({ctx.Request.ServerVariables["LOCAL_ADDR"]})",
                NumberOfUsers = _applicationContext.GetLiveUserCount()
            };
            _logProgramService.UpdateUserLogin(logProgramModel);
        }

        private void FixUserTimeZone(UserOverview customerUser)
        {
            if (customerUser != null && customerUser.TimeZoneId == null)
            {
                // well, if we have AJAX request and user has no TimeZoneId selected in profile,
                // set than local time zone (Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna)... siliently
                customerUser.TimeZoneId = TimeZoneInfo.Local.Id;
            }
        }

        private static TimeZoneAutodetectResult SetUserTimeZone(string timeZoneOffsetInJan1Val, string timeZoneOffsetInJul1Val, UserOverview user)
        {
            var result = TimeZoneAutodetectResult.None;

            int timeZoneOffsetInJan1, timeZoneOffsetInJul1;
            if (int.TryParse(timeZoneOffsetInJan1Val, out timeZoneOffsetInJan1) &&
                int.TryParse(timeZoneOffsetInJul1Val, out timeZoneOffsetInJul1))
            {
                TimeZoneInfo[] timeZones;
                result = TimeZoneResolver.DetectTimeZone(timeZoneOffsetInJan1, timeZoneOffsetInJul1, out timeZones);

                if (string.IsNullOrEmpty(user.TimeZoneId))
                {
                    if (result == TimeZoneAutodetectResult.Failure)
                    {
                        user.TimeZoneId = TimeZoneInfo.Local.Id;
                    }
                    else
                    {
                        user.TimeZoneId = timeZones[0].Id;

                        // we dont want to show any messages to user if we successfully detect his timezone
                        result = TimeZoneAutodetectResult.None;
                    }
                }
                else
                {
                    if (result == TimeZoneAutodetectResult.Failure)
                    {
                        user.TimeZoneId = TimeZoneInfo.Local.Id;
                    }
                    else
                    {
                        if (timeZones.All(it => it.Id != user.TimeZoneId))
                        {
                            // notice to user about to change his time zone in profile
                            result = TimeZoneAutodetectResult.Notice;
                        }
                        else
                        {
                            // no changes in users time zone was made
                            result = TimeZoneAutodetectResult.None;
                        }
                    }
                }
            }
            else
            {
                user.TimeZoneId = TimeZoneInfo.Local.Id;
                result = TimeZoneAutodetectResult.Failure;
            }

            return result;
        }

        private string GetUserNameWithoutDomain(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return userId;

            var index = userId.LastIndexOf(@"\");
            if (index != -1)
                return userId.Substring(index + 1);

            return userId;
        }

        #endregion
    }
}