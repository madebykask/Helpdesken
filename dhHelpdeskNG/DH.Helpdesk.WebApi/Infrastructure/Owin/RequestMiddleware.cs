using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using Microsoft.Owin;

namespace DH.Helpdesk.WebApi.Infrastructure.Owin
{
    public class RequestMiddleware : OwinMiddleware
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        

        public RequestMiddleware(OwinMiddleware next, IApplicationConfiguration applicationConfiguration) 
            : base(next)
        {
            _applicationConfiguration = applicationConfiguration;
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
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = _applicationConfiguration.DefaultCulture;
            log4net.LogicalThreadContext.Properties["requestId"] = new ActivityIdHelper(); //TODO: Move to ILoggerService
            //log4net.LogicalThreadContext.Properties["userInfo"] = new RequestUserInfo();
            log4net.LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo(context.Request);
        }

        private async void EndInvoke(IOwinContext context)
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