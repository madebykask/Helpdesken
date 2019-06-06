using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;
using IpMatcher;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class HelpdeskAuthenticationFilter : IAuthenticationFilter
    {
        public const string SkipAuthResultCheck = "__allowFormsAuth";
        private const string IssueLoginRedirectKey = "__issueLoginRedirectKey";

        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IUserContext _userContext;
        private readonly ILoggerService _logger = LogManager.Session;
        private Matcher _ipMatcher;
        
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

            var loginMode = GetCurrentLoginMode();
            if (loginMode == LoginMode.Mixed)
            {
                InitWinAuthIPMatcher(_applicationConfiguration.WinAuthIPFilter);
            }
        }

        #endregion

        #region Init Methods

        private void InitWinAuthIPMatcher(IList<string> winAuthFilterItems)
        {
            _ipMatcher = null;

            foreach (var item in winAuthFilterItems)
            {
                var ipMask = item.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (ipMask.Length == 2)
                {
                    if (_ipMatcher == null) _ipMatcher = new Matcher(); // create only if there any valid items
                    //ip|netmask
                    _ipMatcher.Add(ipMask[0], ipMask[1]);
                }
            }
        }

        #endregion

        #region IAuthenticationFilter Methods

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            var identity = ctx.User.Identity;
            var isIdentityAuthenticated = identity?.IsAuthenticated ?? false;

            // allow anonymous for login controller actions
            if (IgnoreRequest(filterContext))
            {
                filterContext.HttpContext.Items[SkipAuthResultCheck] = true;
                _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                return;
            }
            
            var customerUserName = _userContext.Login;
            
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
        
        //OnAuthenticationChallenge: is called at the end after other Authorisation filters to be able to process final result - redirect to login page or issue auth challenge
        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            // check if we shall skip auth result check for AllowAnonymous controller actions
            var skipAuthResultCheck = (bool)(context.HttpContext.Items[SkipAuthResultCheck] ?? false);
            if (skipAuthResultCheck)
                return;

            var isIdentityAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            var issueLoginRedirect = Convert.ToBoolean(context.HttpContext.Items[IssueLoginRedirectKey] ?? false);
            
            //do redirect to login page if its authenticated identity but helpdesk user (tblUsers) was not found by identity name so that user could specify his username to login
            if (isIdentityAuthenticated && issueLoginRedirect)
            {
                var loginUrl = _authenticationService.GetLoginUrl(); // specific for each auth mode (win, forms, mixed, sso)
                _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Redirecting to login page: {loginUrl}");
                context.Result = new RedirectResult(loginUrl);
                return;
            }
            
            var loginMode = GetCurrentLoginMode();
            var helpdeskUserIdentity = SessionFacade.CurrentUserIdentity;

            // for mixed mode try to prompt windows login first before redirecting to FormsAuth login 
            if (loginMode == LoginMode.Mixed && string.IsNullOrEmpty(helpdeskUserIdentity?.UserId))
            {
                var clientIP = context.HttpContext.Request.GetIpAddress();
                if (CheckWinAuthIPFilter(clientIP))
                {
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Request ip ({clientIP}) is from WinAuth ip range! Display windows auth.");
                    var loginUrl = _authenticationService.GetLoginUrl();
                    context.Result = new MixedModeWinAuth401Result(loginUrl);
                }
                else
                {
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Request ip ({clientIP}) is not from WinAuth ip range. Do not prompt windows login...");
                }
            }
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
        
        private bool CheckWinAuthIPFilter(string ipAddress)
        {
            if (_ipMatcher == null)
                return true;

            var res = _ipMatcher.MatchExists(ipAddress);
            return res;
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

    #region MixedModeWinAuth401Result

    public class MixedModeWinAuth401Result : ActionResult
    {
        private readonly string _loginPageUrl;

        public MixedModeWinAuth401Result(string loginPageUrl)
        {
            _loginPageUrl = loginPageUrl;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var loginUrl = (_loginPageUrl ?? "/Login/Login").Trim('~');

            // Add script to response to redirect to forms login page in case windows authentication fails
            response.ClearContent();

            var currentRequestUrl = context.HttpContext.Request.Url.PathAndQuery;
            response.Write("<script language=\"javascript\">self.location='" + loginUrl + "?ReturnUrl=" + HttpUtility.UrlEncode(currentRequestUrl) + "';</script>");

            // Required to allow javascript redirection through to browser
            response.TrySkipIisCustomErrors = true;
            response.Status = "401 Unauthorized";
            response.StatusCode = 401;

            // note that the following line is .NET 4.5 or later only
            // otherwise you have to suppress the return URL etc manually!
            response.SuppressFormsAuthenticationRedirect = true;
        }
    }

    #endregion
}