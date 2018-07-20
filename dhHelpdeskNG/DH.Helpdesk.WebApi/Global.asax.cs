using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;

namespace DH.Helpdesk.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        private readonly IApplicationConfiguration _configuration = ManualDependencyResolver.Get<IApplicationConfiguration>();

        protected void Application_Start()
        {
            InitLogging();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = _configuration.DefaultCulture;
            log4net.LogicalThreadContext.Properties["requestId"] = new ActivityIdHelper();//TODO: Move to ILoggerService
            //log4net.LogicalThreadContext.Properties["userInfo"] = new RequestUserInfo();
            log4net.LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
        
        private void InitLogging()
        {
            log4net.Config.XmlConfigurator.Configure(
                new System.IO.FileInfo(
                    HttpContext.Current.Server.MapPath("~/log4net.config")));
        }

    }

    public class ActivityIdHelper
    {
        public override string ToString()
        {
            if (Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            return Trace.CorrelationManager.ActivityId.ToString();
        }
    }

    public class WebRequestInfo
    {
        public override string ToString()
        {
            return HttpContext.Current?.Request?.RawUrl + ", " + HttpContext.Current?.Request?.UserAgent;
        }
    }

    public class RequestUserInfo
    {
        public override string ToString()
        {
            var userName = "unknown";
            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                userName = HttpContext.Current.User.Identity.Name;

            return $"UserId: {userName}";
        }
    }
}
