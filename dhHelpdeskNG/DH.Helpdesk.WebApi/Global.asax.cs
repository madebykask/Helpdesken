using System;
using System.Web;

namespace DH.Helpdesk.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Use this method only if you really know what are you doing. Use Startup.cs instead.

            PreSendRequestHeaders += Application_PreSendRequestHeaders;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("x-aspnet-version");
            Response.Headers.Remove("X-AspNetWebPages-Version");
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }

    }
}
