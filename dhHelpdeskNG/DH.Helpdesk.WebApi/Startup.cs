using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.WebApi.App_Start;
using DH.Helpdesk.WebApi.Infrastructure.Config.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(DH.Helpdesk.WebApi.Startup))]
namespace DH.Helpdesk.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            var container = WebApiConfig.ConfigDiContainer(config);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseCors(CorsOptions.AllowAll);

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            ConfigureAuth(app, config.DependencyResolver);

            WebApiConfig.InitLogging();
            WebApiConfig.Register(config);

            app.Use<RequestMiddleware>();
            app.UseWebApi(config);
        }



    }
}