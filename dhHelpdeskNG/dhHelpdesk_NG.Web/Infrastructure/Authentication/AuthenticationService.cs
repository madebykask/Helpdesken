using System;
using System.Web;
using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public interface IAuthenticationService
    {
        bool Authenticate(HttpContextBase ctx);
        void SignOut(HttpContextBase ctx);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApplicationConfiguration _appConfiguration;
        private readonly IAdfsConfiguration _adfsConfiguration;
        private readonly IUserService _userService;
        private readonly ILogProgramService _logProgramService;
        private readonly IApplicationContext _applicationContext;
        private readonly IUserContext _userContext;
        private readonly ICustomerContext _customerContext;
        private readonly ISessionContext _sessionContext;

        private readonly IAuthenticationBehavior _authenticationBehavior;

        #region ctor()

        public AuthenticationService(IApplicationConfiguration appConfiguration, 
            IAdfsConfiguration adfsConfiguration,
            IAuthenticationServiceBehaviorFactory behaviorFactory,
            ILogProgramService logProgramService,
            IUserService userService,
            IApplicationContext applicationContext,
            ISessionContext sessionContext,
            IUserContext userContext,
            ICustomerContext customerContext)
        {
            _appConfiguration = appConfiguration;
            _adfsConfiguration = adfsConfiguration;
            _userService = userService;
            _userContext = userContext;
            
            _customerContext = customerContext;
            _logProgramService = logProgramService;
            _applicationContext = applicationContext;
            _sessionContext = sessionContext;

            _authenticationBehavior = behaviorFactory.Create(appConfiguration.LoginMode);
        }

        #endregion

        public bool Authenticate(HttpContextBase ctx)
        {
            //authenticate only if context.CurrentUser is not set
            if (_sessionContext.UserIdentity == null) //todo: shall context.CurrentUser be checked instead?
            {
                var loginMode = _appConfiguration.LoginMode;
                _sessionContext.SetLoginMode(loginMode);

                var log = LogManager.Session;
                log.Debug($"AuthenticationService: authenticating user. LoginMode: {loginMode}");

                var userIdentity = _authenticationBehavior.CreateUserIdentity(ctx);

                if (userIdentity != null)
                {
                    ApplyUserIdentityOverrides(userIdentity);
                    _sessionContext.SetUserIdentity(userIdentity);

                    log.Debug($"AuthenticationService: user has successfully been authenticated. " +
                              $"User: {userIdentity.UserId}, Domain: {userIdentity.Domain}, FullName: {userIdentity.FirstName + " " + userIdentity.LastName} ");

                    //get customer user
                    var customerUser = _userService.GetUserByLogin(userIdentity.UserId, null);

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
                        // handle customer user was not found
                        ClearLoginSession(ctx);
                        return false;
                    }
                }
                else
                {
                    ClearLoginSession(ctx);
                    return false;
                }
            }

            return true;
        }

        public void SignOut(HttpContextBase ctx)
        {
            //do login mode specific sign out
            _authenticationBehavior.SignOut(ctx);

            // end user login session
            ClearLoginSession(ctx);
        }

        #region Helper Methods

        private void ApplyUserIdentityOverrides(UserIdentity userIdentity)
        {
            //override userId with value from config
            if (!string.IsNullOrWhiteSpace(_adfsConfiguration.DefaultUserId))
                userIdentity.UserId = _adfsConfiguration.DefaultUserId;

            //override employeeNumber with value from config
            if (!string.IsNullOrWhiteSpace(_adfsConfiguration.DefaultEmployeeNumber))
                userIdentity.EmployeeNumber = _adfsConfiguration.DefaultEmployeeNumber;
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

        private void ClearLoginSession(HttpContextBase ctx)
        {
            var sessionId = _sessionContext.SessionId;
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                _applicationContext.RemoveLoggedInUser(ctx.Session.SessionID);
                _sessionContext.ClearSession();
            }
        }

        #endregion
    }
}