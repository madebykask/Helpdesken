using System.Web.Http;
using log4net.Config;

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
        }
    }
}