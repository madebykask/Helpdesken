using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(DH.Helpdesk.Web.App_Start.Startup))]
namespace DH.Helpdesk.Web.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();           
            ConfigureAuth(app);
            WebApiConfig.Register(config);            
            app.UseWebApi(config);

        }
    }
}