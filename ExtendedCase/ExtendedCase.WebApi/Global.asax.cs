using log4net.Config;
using System.Web.Http;
using System;
using System.Web;

namespace ExtendedCase.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //init log4net
            XmlConfigurator.Configure();

            //init web api
            GlobalConfiguration.Configure(WebApiConfig.Register);

            PreSendRequestHeaders += Application_PreSendRequestHeaders;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNetWebPages-Version");
            Response.Headers.Remove("X-AspNet-Version");
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //Use this method only if you really know what are you doing. Use RequestMiddleware.cs instead.
        }
    }
}