using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IUserContext _userContext;
        private readonly ILoggerService _logger = LogManager.Session;
        private bool _allowRequest;

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
            var customerUserName = _userContext.Login;

            var loginMode = InitCurrentLoginMode();

            _logger.Debug($"AuthenticationFilter called. CustomerUser: {customerUserName}, Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");

            // allow anonymous for login controller actions
            if (this.IgnoreRequest(filterContext))
            {
                _allowRequest = true;
                _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                return;
            }

            //perform signin for helpdesk customer user only if request has been authenticated by native mechanisms (forms, wins, adfs,..)
            if (identity != null && identity.IsAuthenticated && string.IsNullOrEmpty(_sessionContext.UserIdentity?.UserId))
            {
                _logger.Debug($"AuthenticationFilter. Performing signIn. Identity: {identity?.Name}");
                var isAuthenticated = _authenticationService.SignIn(ctx);
                if (!isAuthenticated)
                {
                    _logger.Warn($"AuthenticationFilter. Failed to sign in. Signing out. Identity: {identity?.Name}");
                    _authenticationService.ClearLoginSession(ctx);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            var isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated && context.Result != null)
            {
                if (context.Result is HttpUnauthorizedResult || 
                    (context.Result is HttpStatusCodeResult && ((HttpStatusCodeResult)context.Result).StatusCode == 401))
                {
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Redirecting to login page.");
                    context.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { action = "Login", controller = "Login" }));
                }
                else if (_applicationConfiguration.LoginMode == LoginMode.SSO)
                {
                    _logger.Warn($"AuthenticationFilter.OnAuthenticationChallenge. Redirecting to login page.");
                    context.Result = null;
                }
            }
        }

        #endregion

        #region Helper Methods

        private bool IgnoreRequest(AuthenticationContext filterContext)
        {
#if DEBUG
            var requestUrl = filterContext.HttpContext.Request.Url?.ToString() ?? string.Empty;
            if (requestUrl.IndexOf("_sts") != -1)
                return true;
#endif
            return filterContext.ActionDescriptor
                .GetCustomAttributes(inherit: true)
                .OfType<AllowAnonymousAttribute>()
                .Any();
        }

        private LoginMode InitCurrentLoginMode()
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