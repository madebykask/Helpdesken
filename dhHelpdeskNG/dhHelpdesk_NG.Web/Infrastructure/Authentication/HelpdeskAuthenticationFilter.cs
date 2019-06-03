using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class HelpdeskAuthenticationFilter : IAuthenticationFilter
    {
        public const string AllowFormsAuthKey = "__allowFormsAuth";
        private const string IssueLoginRedirectKey = "__issueLoginRedirectKey";

        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IUserContext _userContext;
        private readonly ILoggerService _logger = LogManager.Session;

        #region ctor()

        public HelpdeskAuthenticationFilter(IAuthenticationService authenticationService, 
            ISessionContext sessionContext,
            IUserContext userContext,
            IApplicationConfiguration applicationConfiguration)
        {
            _authenticationService = authenticationService;
            _sessionContext = sessionContext;
            _userContext = userContext;
            _applicationConfiguration = applicationConfiguration;
        }

        #endregion

        #region IAuthenticationFilter Methods

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var identity = ctx.User.Identity;
            var isIdentityAuthenticated = identity?.IsAuthenticated ?? false;

            // allow anonymous for login controller actions
            if (this.IgnoreRequest(filterContext))
            {
                _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                return;
            }
            
            var customerUserName = _userContext.Login;
            var loginMode = GetCurrentLoginMode();

            _logger.Debug($"AuthenticationFilter called. CustomerUser: {customerUserName}, Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
            
            //NOTE: perform signin for helpdesk customer user only if request has been authenticated by native mechanisms (forms, wins, adfs,..)
            //      but helpdesk user has not been created yet or doesn't exist in session any more
            if (isIdentityAuthenticated && string.IsNullOrEmpty(_sessionContext.UserIdentity?.UserId))
            {
                _logger.Debug($"AuthenticationFilter. Performing user signIn. Identity: {identity?.Name}");
                var isUserAuthenticated = _authenticationService.SignIn(ctx);
                if (!isUserAuthenticated)
                {
                    _logger.Warn($"AuthenticationFilter. Failed to sign in user. Signing out. Identity: {identity?.Name}");
                    _authenticationService.ClearLoginSession(ctx);
                    filterContext.HttpContext.Items[IssueLoginRedirectKey] = true; 
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
        
        //OnAuthenticationChallenge is called at the end after other Authorisation filters to be able to process final result
        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            var isIdentityAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            var issueLoginRedirect = Convert.ToBoolean(context.HttpContext.Items[IssueLoginRedirectKey] ?? false);
            
            //do redirect to login page if its authenticated identity but helpdesk user (tblUsers) was not found by identity name so that user could specify his username to login
            if (isIdentityAuthenticated && issueLoginRedirect)
            {
                //if (context.Result is HttpUnauthorizedResult || (context.Result is HttpStatusCodeResult && ((HttpStatusCodeResult)context.Result).StatusCode == 401))
                {
                    var loginUrl = _authenticationService.GetLoginUrl();
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Redirecting to login page: {loginUrl}");
                    context.HttpContext.Items[AllowFormsAuthKey] = true;
                    context.Result = new RedirectResult(loginUrl);
                }
            }
            
            //todo: check if it can be implemented here
            //var loginMode = GetCurrentLoginMode();
            //if (context.Result is HttpUnauthorizedResult && loginMode == LoginMode.Mixed)
            //{
            //    var loginUrl = _authenticationService.GetLoginUrl();
            //    if (!string.IsNullOrEmpty(loginUrl))
            //    {
            //        _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Unauthorised. Redirecting to login page: {loginUrl}.");
            //        context.HttpContext.Items[AllowFormsAuthKey] = true;
            //        context.Result = new RedirectResult(loginUrl);
            //    }
            //}
        }

        #endregion

        #region Helper Methods

        private bool IgnoreRequest(AuthenticationContext filterContext)
        {
#if DEBUG
            // need to ignore request for local EmbeddedSts server
            var requestUrl = filterContext.HttpContext.Request.Url?.ToString() ?? string.Empty;
            if (requestUrl.IndexOf("_sts") != -1)
                return true;
#endif

            //todo: check 
            //if (_applicationConfiguration.LoginMode == LoginMode.Mixed) return false;

            return filterContext.ActionDescriptor
                .GetCustomAttributes(inherit: true)
                .OfType<AllowAnonymousAttribute>()
                .Any();
        }

        private LoginMode GetCurrentLoginMode()
        {
            if (_sessionContext.LoginMode == LoginMode.None)
            {
                _sessionContext.SetLoginMode(_applicationConfiguration.LoginMode);
            }
            return _sessionContext.LoginMode;
        }

        #endregion
    }
}