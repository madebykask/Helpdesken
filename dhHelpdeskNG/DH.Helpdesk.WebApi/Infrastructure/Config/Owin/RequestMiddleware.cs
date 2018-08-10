using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Owin
{
    public class RequestMiddleware : OwinMiddleware
    {
        public RequestMiddleware(OwinMiddleware next) 
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            BeginInvoke(context);
            await Next.Invoke(context);
            EndInvoke(context);
        }

        private void BeginInvoke(IOwinContext context)
        {
            // Do custom work before controller execution

            var requestScope = context.GetAutofacLifetimeScope();
            var configuration = requestScope.Resolve<IApplicationConfiguration>();
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = configuration.DefaultCulture;
            log4net.LogicalThreadContext.Properties["requestId"] = new ActivityIdHelper();//TODO: Move to ILoggerService
            //log4net.LogicalThreadContext.Properties["userInfo"] = new RequestUserInfo();
            log4net.LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo(context.Request);
        }

        private void EndInvoke(IOwinContext context)
        {
            // Do custom work after controller execution
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
        private readonly IOwinRequest _owinRequest;
        
        public WebRequestInfo(IOwinRequest owinRequest)
        {
            _owinRequest = owinRequest;
        }

        public override string ToString()
        {
            return _owinRequest?.Uri?.AbsolutePath + ", " + _owinRequest?.Headers["user-agent"];
        }
    }

    public class RequestUserInfo
    {
        private readonly IOwinRequest _owinRequest;

        public RequestUserInfo(IOwinRequest owinRequest)
        {
            _owinRequest = owinRequest;
        }

        public override string ToString()
        {
            var userName = "unknown";
            if (_owinRequest != null && _owinRequest.User.Identity.IsAuthenticated)
                userName = _owinRequest.User.Identity.Name;

            return $"UserId: {userName}";
        }
    }
}