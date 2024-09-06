using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using DH.Helpdesk.BusinessData.Enums.Users;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class HelpdeskAuthenticationFilter : IAuthenticationFilter
    {
        public const string SkipAuthResultCheck = "__skipAuthResultCheck";
        private const string IssueLoginRedirectKey = "__issueLoginRedirectKey";
        
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IUserContext _userContext;
        private readonly ILoggerService _logger = LogManager.Session;
        private readonly IAuthenticationServiceBehaviorFactory _authenticationBehaviorFactory;
        private Matcher _ipMatcher;
        #region ctor()

        public HelpdeskAuthenticationFilter(IAuthenticationService authenticationService, 
            ISessionContext sessionContext,
            IUserContext userContext,
            IApplicationConfiguration applicationConfiguration,
            IAuthenticationServiceBehaviorFactory authenticationBehaviorFactory)
        {
            _authenticationService = authenticationService;
            _sessionContext = sessionContext;
            _userContext = userContext;
            _applicationConfiguration = applicationConfiguration;
            _authenticationBehaviorFactory = authenticationBehaviorFactory;

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

            var loginMode = GetCurrentLoginMode();

            if (IgnoreRequest(filterContext))
            {
                filterContext.HttpContext.Items[SkipAuthResultCheck] = true;
                _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                return;
            }
            if (isIdentityAuthenticated && SessionFacade.CurrentUser != null)
            {
                return;
            }

            

            if (loginMode == LoginMode.Microsoft)
            {
                //Working
                _authenticationService.SetLoginModeToMicrosoft();
                if(SessionFacade.CurrentUser == null)
                {
                    if (_authenticationService.SignIn(ctx))
                    {    
                        //If user clicked a link not logged in
                        if (ctx.Request.Path != "/")
                        {
                            filterContext.Result = new RedirectResult(ctx.Request.Path);
                        }
                        else
                        {
                            //Send to preferred startpage
                            if ((StartPage)SessionFacade.CurrentUser.StartPage == StartPage.CaseSummary)
                            {
                                filterContext.Result = new RedirectResult("~/Cases");
                            }
                            else if ((StartPage)SessionFacade.CurrentUser.StartPage == StartPage.AdvancedSearch)
                            {
                                filterContext.Result = new RedirectResult("~/Cases/AdvancedSearch");
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("~/");
                            }
                            return;
                        }
                        
                    }
                    else
                    {
                        ctx.Session.Abandon();
                        var loginUrl = "~/Login/Login";
                        OnError(filterContext, loginUrl);
                        return;
                    }
                }
            }

            else if (loginMode == LoginMode.Application && !string.IsNullOrEmpty(_userContext.Login))
            {

                var userId = _userContext.UserId;
                _authenticationService.SetLoginModeToApplication();
                _authenticationService.SignInApplicationUser(ctx, userId);
                return;
            }
            else
            {
                // allow anonymous for login controller actions
                if (IgnoreRequest(filterContext))
                {
                    filterContext.HttpContext.Items[SkipAuthResultCheck] = true;
                    _logger.Debug($"AuthenticationFilter. Skip check for anonymous action. Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");
                    return;
                }

                var customerUserName = _userContext.Login;

                _logger.Debug($"AuthenticationFilter called. CustomerUser: {customerUserName}, Identity: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}, Url: {ctx.Request.Url}");

                //Windows login
                if (isIdentityAuthenticated)
                {
                    //NOTE: perform sign in only if request is authenticated (forms, wins, adfs,..) but helpdesk user was not created yet (first login request) or doesn't exist in session any more (asp.net session expired)
                    if (string.IsNullOrEmpty(_sessionContext.UserIdentity?.UserId))
                    {
                        _logger.Debug($"AuthenticationFilter. Performing user signIn. Identity: {identity?.Name}");
                        var isUserAuthenticated = _authenticationService.SignIn(ctx);
                        if (!isUserAuthenticated)
                        {
                            _logger.Warn($"AuthenticationFilter. Failed to sign in user. Signing out. Identity: {identity?.Name}");
                            _authenticationService.ClearLoginSession(ctx);

                            //redirect to forms login page if user cannot be found in the database by identity user name
                            //Add redirect to prefferred page
                            string requestedUrl = "~" + ctx.Request.Path;
                            var redirectUrl = HttpUtility.UrlEncode(requestedUrl);
                            var loginUrl = "~/Login/Login";
                            if (!string.IsNullOrEmpty(requestedUrl) && requestedUrl != "~/" && !requestedUrl.ToLower().Contains("login/login"))
                            {
                                loginUrl = loginUrl + "?returnUrl=" + redirectUrl;
                            }

                            _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Redirecting to login page: {loginUrl}");
                            OnError(filterContext, loginUrl);
                        }
                        else
                        {
                            //If user clicked a link not logged in
                            if (ctx.Request.Path != "/")
                            {
                                filterContext.Result = new RedirectResult(ctx.Request.Path);
                            }
                            else
                            {
                                //Send to preferred startpage
                                if ((StartPage)SessionFacade.CurrentUser.StartPage == StartPage.CaseSummary)
                                {
                                    filterContext.Result = new RedirectResult("~/Cases");
                                }
                                else if ((StartPage)SessionFacade.CurrentUser.StartPage == StartPage.AdvancedSearch)
                                {
                                    filterContext.Result = new RedirectResult("~/Cases/AdvancedSearch");
                                }
                                else
                                {
                                    filterContext.Result = new RedirectResult("~/");
                                }
                                return;
                            }
                        }

                    }

                }
                else
                {
                    _authenticationService.ClearLoginSession(ctx);
                    string requestedUrl = "~" + ctx.Request.Path;
                    var redirectUrl = HttpUtility.UrlEncode(requestedUrl);
                    var loginUrl = "~/Login/Login";
                    if (!string.IsNullOrEmpty(requestedUrl) && requestedUrl != "~/" && !requestedUrl.ToLower().Contains("login/login"))
                    {
                        loginUrl = loginUrl + "?returnUrl=" + redirectUrl;
                    }

                    OnError(filterContext, loginUrl);
                }
            }

        }

        private static void OnError(AuthenticationContext filterContext, string loginUrl)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                if (filterContext.HttpContext.Request.Url != null)
                    filterContext.Result = new ContentResult()
                    {
                        Content = urlHelper.Action("Index", "Home", null,
                            filterContext.HttpContext.Request.Url.Scheme),
                    };
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.StatusDescription = "You are not authorized";
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
            else
            {
                filterContext.Result = new RedirectResult(loginUrl);
            }
        }

        //OnAuthenticationChallenge: is called at the end after other Authorisation filters to be able to process final result - redirect to login page or issue auth challenge
        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            var loginMode = GetCurrentLoginMode();
            var isIdentityAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            //Todo - Check with Håkan
            // check if we shall skip auth result check for AllowAnonymous controller actions
            var skipAuthResultCheck = (bool)(context.HttpContext.Items[SkipAuthResultCheck] ?? false);
            if (skipAuthResultCheck)
                return;

            var httpUserIdentity = context.HttpContext.User?.Identity?.Name ?? string.Empty;
            var helpdeskUserIdentity = SessionFacade.CurrentUserIdentity?.UserId ?? string.Empty;


            _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge: {context.HttpContext.Request.Url}. AuthMode: {loginMode}. [IsAuthenticated: {isIdentityAuthenticated}, HttpUserIdentity: {httpUserIdentity}, HelpdeskSessionUser: {helpdeskUserIdentity}]");

            #region MixedMode handling

            // for mixed mode try to use windows login first before redirecting to helpdesk (FormsAuth) login page
            if (loginMode == LoginMode.Mixed && !isIdentityAuthenticated)
            {
                var loginUrl = _authenticationService.GetSiteLoginPageUrl();
                var clientIP = context.HttpContext.Request.GetIpAddress();
                if (CheckWinAuthIPFilter(clientIP))
                {
                    // return 401 challenge to show windows login page. Passing login url param to redirect if a user cancels windows login.
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Request ip ({clientIP}) is from WinAuth ip range! Display windows auth.");
                    context.Result = new MixedModeWinAuth401Result(loginUrl);
                }
                else
                {
                    // redirect to forms login page if its external user to login with helpdesk credentials
                    context.Result = new RedirectResult(loginUrl);
                    _logger.Debug($"AuthenticationFilter.OnAuthenticationChallenge. Request ip ({clientIP}) is not from WinAuth ip range. Do not prompt windows login...");
                }
            }


            #endregion
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