using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Owin;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;

namespace DH.Helpdesk.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Use this method only if you really know what are you doing. Use Startup.cs instead.
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }

        //protected void Application_PostAuthorizeRequest()
        //{
        //    if (IsWebApiRequest())
        //    {
        //        HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //    }
        //}

        //private bool IsWebApiRequest()
        //{
        //    return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        //}
        


    }

}
