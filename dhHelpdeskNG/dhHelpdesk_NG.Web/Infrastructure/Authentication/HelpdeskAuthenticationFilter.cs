using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
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
        public const string AllowFormsAuthKey = "__allowFormsAuth";

        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IUserContext _userContext;
        private readonly ILoggerService _logger = LogManager.Session;

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

        #region IAuthenticationFilter Methods

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var identity = ctx.User.Identity;
            var customerUserName = _userContext.Login;

            var loginMode = InitCurrentLoginMode();

            _logger.Debug($"AuthenticationFilter called. CustomerUser: {customerUserName}, Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");

            // allow anonymous for login controller actions
            if (this.IsAnonymousAction(filterContext))
            {
                _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                return;
            }

            // restore forms identity principal
            if (loginMode == LoginMode.Application && identity is WindowsIdentity)
            {
                //windows authentication module sets identity first
                if (TryRestoreFormsIdentity(ctx))
                {
                    identity = ctx.User.Identity;
                }
            }

            //perform signin for helpdesk customer user 
            if (identity != null && identity.IsAuthenticated && string.IsNullOrEmpty(_sessionContext.UserIdentity?.UserId))
            {
                _logger.Debug($"AuthenticationFilter. Performing signIn. Identity: {identity?.Name}");
                var isAuthenticated = _authenticationService.SignIn(ctx);
                if (!isAuthenticated)
                {
                    _logger.Warn($"AuthenticationFilter. Failed to sign in. Signing out. Identity: {identity?.Name}");

                    _authenticationService.SignOut(ctx);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        private bool TryRestoreFormsIdentity(HttpContextBase ctx)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                var authCookie = ctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    _logger.Debug("AuthenticationFilter. Restoring forms identity.");

                    try
                    {
                        var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        ctx.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
                        _logger.Debug($"AuthenticationFilter. FormsIdentity has been restored. UserId: {ctx.User.Identity?.Name}");
                        return true;
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"AuthenticationFilter. Failed to restore forms identity. {e.Message}");
                    }
                }
            }

            return false;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            //redirect to login page if authentication failed during OnAuthentication method call
            if (context.Result is HttpUnauthorizedResult)
            {
                var loginUrl = _authenticationService.GetLoginUrl();
                if (!string.IsNullOrEmpty(loginUrl))
                {
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Unauthorised. Redirecting to login page: {loginUrl}.");
                    context.HttpContext.Items[AllowFormsAuthKey] = true;
                    context.Result = new RedirectResult(loginUrl);
                }
            }
        }

        #endregion

        #region Helper Methods

        private bool IsAnonymousAction(AuthenticationContext filterContext)
        {
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