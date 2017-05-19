using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Ninject.Web.WebApi;

[assembly: OwinStartup(typeof(DH.Helpdesk.Web.App_Start.Startup))]
namespace DH.Helpdesk.Web.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Filters.Add(new AuthorizeAttributeExtended());
            ConfigureAuth(app);
            WebApiConfig.Register(config);            
            var kernel = NinjectWebCommon.Bootstrapper.Kernel;                        
            config.DependencyResolver = new NinjectDependencyResolver(kernel);            
            app.UseWebApi(config);
        }
    }       
}