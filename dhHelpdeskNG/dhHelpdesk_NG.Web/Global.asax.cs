using System.Configuration;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Security;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

namespace DH.Helpdesk.Web
{
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using DH.Helpdesk.Common.Logger;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Binders;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Infrastructure.Logger;

    using Microsoft.Practices.ServiceLocation;
    using System.Diagnostics;
    using Infrastructure;
    using Services.Services.Concrete;
    using BusinessData.Models.LogProgram;

    public class MvcApplication : HttpApplication
    {
        private readonly IConfiguration configuration = ManualDependencyResolver.Get<IConfiguration>();
        
        protected void Application_Start()
        {
            //Debugger.Launch();

            AreaRegistration.RegisterAllAreas();
            //MARK: Remove old Api
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            ViewEngineInit();

            RegisterLocalizedAttributes();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //MARK: Remove old Api
            //FilterConfig.RegisterWebApiGlobalFilters(GlobalConfiguration.Configuration.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterBinders();
            ProcessStartupTasks();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JsonFormatConfig.ConfigWebApi();
            JsonFormatConfig.ConfigMVC();

            ECT.FormLib.FormLibSetup.Setup();
            ECT.FormLib.FormLibSetup.SetupRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
            //System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;//uncomment this if XFrameOptions is added in web.config headers            
            // ECT.FormLib.FormLibSetup.Setup(); todo

            #if DEBUG
                BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif

            //fix for adfs(sso) claims-based identity
            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = System.Security.Claims.ClaimTypes.Name;

            if (IsSsoMode())
            {
                FederatedAuthenticationConfiguration.Configure();
            }
        }

        private void ViewEngineInit()
        {
            // Clear all registered view engines
            ViewEngines.Engines.Clear();

            // Add back in just the Razor view engine
            var viewEngine = new RazorViewEngine
            {
                ViewLocationCache = new DefaultViewLocationCache(TimeSpan.FromHours(24))
            };

            ViewEngines.Engines.Add(viewEngine);
        }

        protected void Application_PostAuthorizeRequest()
        {
            //MARK: Remove old Api
            //if (IsWebApiRequest())
            //{
            //	HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            //}
        }

        //MARK: Remove old Api
        //private bool IsWebApiRequest()
        //{
        //	return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains(@"/" + WebApiConfig.UrlPrefixRelative + @"/");
        //}        

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = this.configuration.Application.DefaultCulture;
            //LogSession("Application.BeginRequest.", Context);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var configuration = DependencyResolver.Current.GetService<IApplicationConfiguration>();
            var loginUrl = (FormsAuthentication.LoginUrl ?? "/Login/Login").Trim('~');

            LogSession($"Application.EndRequest. MixedAuthMode: {configuration.MixedAuthMode}, Status: {Response.Status}, StatusCode: {Response.StatusCode}, RedirectLocation: {Response.RedirectLocation}", Context);

            // if we are running in a mixed mode (win + forms) - we need to handle Forms auth unauthorised redirect request (Status:302).
            if (this.Response.StatusCode == 302 &&
                configuration.LoginMode == LoginMode.Application && 
                configuration.MixedAuthMode &&
                this.Response.RedirectLocation.IndexOf(loginUrl, StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                //allow forms auth redirect if it was triggered by HelpdeskAuthenticationFilter 
                if (AllowFormsAuthenticationRedirect(Context))
                {
                    LogSession("Application.EndRequest: allow authorisation redirect by forms auth filter.", Context);
                    return;
                }
                    
                LogSession("Application.EndRequest: Cancelling unauthorised redirect issued by forms authentication.", Context);
                //if (Request.Browser.Win32)
                {
                    // Add script to response to redirect to forms login page in case windows authentication fails
                    this.Response.ClearContent();
                    this.Response.Write("<script language=\"javascript\">self.location='" + loginUrl + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery) + "';</script>");

                    // Required to allow javascript redirection through to browser
                    this.Response.TrySkipIisCustomErrors = true;
                    this.Response.Status = "401 Unauthorized";
                    this.Response.StatusCode = 401;

                    // note that the following line is .NET 4.5 or later only
                    // otherwise you have to suppress the return URL etc manually!
                    this.Response.SuppressFormsAuthenticationRedirect = true;
                }
            }
        }

        #region Authentication Events 

        protected void WindowsAuthentication_OnAuthenticate(object sender, WindowsAuthenticationEventArgs args)
        {
            var identity = args.Identity;
            LogSession($">>>WindowsAuthentication.OnAuthentication event. Idenitty: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, IsAnonymous: {identity?.IsAnonymous ?? false}", Context);
        }

        protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        {
            var identity = args.User?.Identity;
            LogSession($">>>FormsAuthentication.OnAuthenticate event. Idenitty: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}", Context);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs args)
        {
            var identity = Context.User?.Identity;
            LogSession($">>>Application.PostAuthenticateRequest event. Idenitty: {identity?.Name}, Authenticated: {identity?.IsAuthenticated ?? false}, AuthType: {identity?.AuthenticationType}", Context);
        }

        #endregion

        #region ADFS Module Events 

        protected void Session_Start(object sender, EventArgs e)
        {
            var sessionId = Session.SessionID;
            LogSession("Session.Start: " + sessionId, Context);
        }

        protected void WSFederationAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs e)
        {
            LogSession($"WSFederationAuthenticationModule.SessionSecurityTokenCreated: token created.", Context);
        }
        
        protected void WSFederationAuthenticationModule_SecurityTokenValidated(object sender, SecurityTokenValidatedEventArgs e)
        {
            LogSession($"WSFederationAuthenticationModule.SecurityTokenValidated: token has been validated. {e.ClaimsPrincipal}", Context);
        }

        protected void WSFederationAuthenticationModule_SignedIn(object sender, EventArgs e)
        {
            LogIdentityClaims(Context);
            LogSession($"WSFederationAuthenticationModule.SignedIn: user signed In.", Context);
        }

        protected void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            var redirectUrl = e.SignInRequestMessage.WriteQueryString();
            LogSession($"WSFederationAuthenticationModule.RedirectingToIdentityProvider. Redirect to sts - {redirectUrl}", Context);
        }

        protected void WSFederationAuthenticationModule_SignInError(object sender, ErrorEventArgs e)
        {
            LogSession($"WSFederationAuthenticationModule.SignInError: sign in error. {e.Exception?.Message ?? "Unknown"}", Context);
        }

        protected void WSFederationAuthenticationModule_AuthorizationFailed(object sender, AuthorizationFailedEventArgs e)
        {
            LogSession($"WSFederationAuthenticationModule.AuthorizationFailed. Authorisation failed.", Context);
        }
        
        #region SessionAuthenticationModule events

        protected void SessionAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs args)
        {
            var sessionToken = args.SessionToken;
            LogSession($"SessionAuthenticationModule.SessionSecurityTokenCreated. Token valid: {sessionToken.ValidFrom} - {sessionToken.ValidTo}.", Context);
        }

        protected void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs args)
        {
            var token = args.SessionToken;
            LogSession($"SessionAuthenticationModule.SessionSecurityTokenReceived. Token valid: {token.ValidFrom} - {token.ValidTo}.", Context);
            
            var configuration = ManualDependencyResolver.Get<IAdfsConfiguration>();
            var federationAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();

            //check if token has expired:
            if (token.ValidTo < DateTime.UtcNow)
            {
                LogSession($"Security token lifetime has been expired: ValidTo: {token.ValidTo}, UtcNow: {DateTime.UtcNow}. Signing out.", Context);
                SessionFacade.ClearSession();
                federationAuthenticationService.SignOut(null);
                return;
            }

            if (configuration.EnableSlidingExpiration)
            {
                LogSession("Try to refresh adfs Token lifetime.", Context);

                var sam = (SessionAuthenticationModule)sender;
                var refreshedToken =
                    federationAuthenticationService.RefreshSecurityTokenLifeTime(sam, args.SessionToken, configuration.SecurityTokenMaxDuration);

                if (refreshedToken != null)
                {
                    args.SessionToken = refreshedToken;
                    args.ReissueCookie = true;
                    LogSession($"Adfs Token lifetime has been refreshed to: {refreshedToken.ValidTo},", Context);
                }
            }
        }

        #endregion

        private void LogSession(string msg, HttpContext ctx)
        {
            var request = ctx.Request;
            var identity = ctx.User?.Identity;
            var isAuthenticated = identity?.IsAuthenticated ?? false;
            var contextInfo = $"Authenticated: {isAuthenticated}, User: {identity?.Name}, Request: {request.Url}";

            var logger = LogManager.Session;
            logger.Debug($"{msg} {contextInfo}");
        }

        private void DumpSessionState(HttpContext ctx)
        {
            try
            {
                var session = ctx.Session;
                if (!string.IsNullOrEmpty(session?.SessionID))
                {
                    Trace.WriteLine($">>> [{DateTime.Now}] Helpdesk.Session. Id: {session.SessionID}.Request: {ctx.Request.Url}");

                    var logSessionKeys = ConfigurationManager.AppSettings["helpdesk.logsessionkeys"];
                    if (!string.IsNullOrEmpty(logSessionKeys))
                    {
                        var strBld = new StringBuilder();
                        foreach (string key in session.Keys)
                        {
                            if (logSessionKeys == "*" || logSessionKeys.Equals(key, StringComparison.OrdinalIgnoreCase))
                            {
                                strBld.AppendFormat("\t- {0}: {1}{2}", key, session[key], Environment.NewLine);
                            }
                        }

                        Trace.WriteLine(strBld.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError($"DumpSessionState: Unknown error. Error: {ex.Message}" );
            }
        }

        #endregion

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            DumpSessionState(Context);

            try
            {
                var session = HttpContext.Current.Session;

                // Check can be null in some request (static)
                if (session != null)
                {
                    var sessionStatus = session["SessionStatus"] as SessionStatus;

                    //LogSession($"Application.AcquireRequestState called. IsCaseDataChanged: {SessionFacade.IsCaseDataChanged}", Context);

                    var currentUser = SessionFacade.CurrentUser;
                    var userID = currentUser != null ? currentUser.Id : (int?)null;

                    // No user session status exist, create new one
                    if (sessionStatus == null)
                    {
                        int responseTime = CheckResponseTime();
                        sessionStatus = new SessionStatus
                        {
                            LastSessionStatusUpdate = DateTime.Now,
                            Guid = Guid.NewGuid()
                        };
                        session["SessionStatus"] = sessionStatus;

                        using (var p = Process.GetCurrentProcess())
                        {
                            // Start log for session
                            Log(userID, responseTime, sessionStatus.Guid, p.WorkingSet64);
                        }
                    }
                    else // A status session item does already exist
                    {
                        var now = DateTime.Now;

                        // Check if is time to make update
                        var expires = sessionStatus.LastSessionStatusUpdate.AddSeconds(SessionStatus.SessionRefreshTime);
                        if (now >= expires)
                        {
                            // Updates sesstion status with current status information
                            var responseTime = CheckResponseTime();
                            sessionStatus.LastSessionStatusUpdate = now;

                            using (var p = Process.GetCurrentProcess())
                            {                                
                                Log(userID, responseTime, sessionStatus.Guid, p.WorkingSet64);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore all errors here
            }
        }

        private void Log(int? userId, int responseTime, Guid guid, long memoryUsage)
        {
            try
            {
                var _logProgramService = ManualDependencyResolver.Get<ILogProgramService>();
                var data = $"{{ \"Topic\": \"Top 100 tblCases\", \"ResponseTime\": {responseTime}, \"Memory\": {memoryUsage}, \"Guid\": \"{guid}\" }}";

                var logProgramModel = new LogProgram
                {
                    CaseId = 0,
                    CustomerId = SessionFacade.CurrentCustomer != null ? SessionFacade.CurrentCustomer.Id : 0,
                    LogType = 99, // New Type for test purpose 
                    LogText = data,
                    New_Performer_user_Id = 0,
                    Old_Performer_User_Id = "0",
                    RegTime = DateTime.UtcNow,
                    UserId = userId,
                    ServerNameIP = $"{Environment.MachineName} ({Request.ServerVariables["LOCAL_ADDR"]})",
                    NumberOfUsers = null
                };

                _logProgramService.UpdateUserLogin(logProgramModel);            
            }   
            catch (Exception ex)
            {
                
            }    
        }

        protected int CheckResponseTime()
        {            
            var sw = new Stopwatch();

            try
            {
                sw.Start();
                var _caseService = ManualDependencyResolver.Get<ICaseService>();
                _caseService.GetTop100CasesForTest();
                sw.Stop();
                return sw.Elapsed.Milliseconds;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        class SessionStatus
        {
            // Seconds between refreshes, suggestion each 30 minutes, add to some config
            public const int SessionRefreshTime = 1800;
            public DateTime LastSessionStatusUpdate { get; set; }
            public Guid Guid { get; internal set; }
        }


#if !DEBUG
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;

            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            var currentAction = " ";

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();

            var controller = new ErrorController(ManualDependencyResolver.Get<IMasterDataService>());
            var routeData = new RouteData();
            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;

                    // others if any
                    default:
                        action = "Index";
                        break;
                }
            }

            if (ex is BusinessLogicException)
            {
                action = "BusinessLogicError";
            }

            var workContext = ManualDependencyResolver.Get<IWorkContext>();
            var guid = Guid.NewGuid();
            LogManager.Error.Error(new ErrorContext(
                                        guid,
                                        ex,
                                        currentController,
                                        currentAction,
                                        httpContext,
                                        workContext).ToString());


            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new DH.Helpdesk.Web.Models.Error.HandleErrorInfoGuid(ex, currentController, currentAction, guid);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
#endif

        /// <summary>
        /// The register binders.
        /// </summary>
        private static void RegisterBinders()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
        }

        private static void RegisterLocalizedAttributes()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(LocalizedRequiredAttribute),
                typeof(RequiredAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(LocalizedStringLengthAttribute),
                typeof(StringLengthAttributeAdapter));
        }

        /// <summary>
        /// process startup tasks.
        /// </summary>
        private static void ProcessStartupTasks()
        {
            ILoggerService errorLoggerService = LogManager.Error;
            foreach (var task in ServiceLocator.Current.GetAllInstances<IStartUpTask>())
            {
                try
                {
                    if (task != null && task.IsEnabled)
                    {
                        task.Configure();
                    }
                }
                catch (Exception ex)
                {
                    errorLoggerService.Error(ex);
                }
            }
        }

        private static bool IsSsoMode()
        {
            var appSettingsProvider = new ApplicationConfiguration();
            return appSettingsProvider.LoginMode == LoginMode.SSO;
        }
        private static void LogIdentityClaims(HttpContext ctx)
        {
            var log = LogManager.Session;
            var claimsIdentity = ctx.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null && claimsIdentity.Claims.Any())
            {
                log.Debug(">>> Claims found:");
                foreach (var claim in claimsIdentity.Claims)
                {
                    log.Debug($"Claim: {claim.Type}, value: {claim.Value}, Issuer: {claim.Issuer}");
                }
            }
        }

        private bool AllowFormsAuthenticationRedirect(HttpContext ctx)
        {
            var key = HelpdeskAuthenticationFilter.AllowFormsAuthKey;
            if (ctx.Items.Contains(key))
            {
                var val = Convert.ToBoolean(ctx.Items[key] ?? false);
                return val;
            }

            return false;
        }
    }
}