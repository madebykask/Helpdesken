using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.WebApi.Infrastructure.Owin;
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

            //register cors for owin components (auth provider) only 
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider()
                {
                    PolicyResolver = request =>
                        (request.Path.Value ?? string.Empty).Equals("/token", StringComparison.OrdinalIgnoreCase)
                            ? Task.FromResult(new CorsPolicy
                            {
                                AllowAnyHeader = true,
                                AllowAnyMethod = true,
                                AllowAnyOrigin = true,
                                SupportsCredentials = false,
                                PreflightMaxAge = 600
                            })
                            : Task.FromResult<CorsPolicy>(null)
                }
            });

            RegisterMiddlewareComponents(container, app);

            app.UseAutofacWebApi(config);
            ConfigureAuth(app, config.DependencyResolver);

            WebApiConfig.InitLogging();
            WebApiConfig.Register(config);

            app.UseWebApi(config);

            config.AddApiVersioning( o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });

            SwaggerConfig.Register(config);
            //configuration.AddVersionedApiExplorer(//TODO: install nuget and uncomment when ApiExplorer will be used
        }

        private void RegisterMiddlewareComponents(IContainer container, IAppBuilder app)
        {
            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            //app.UseAutofacMiddleware(container); // for this approach compomnents should be registered withing the container builder

            // alternative to UseAutofacMiddleware - to control order of pipeline
            app.UseAutofacLifetimeScopeInjector(container);
            
            // middleware in specific order
            app.UseMiddlewareFromContainer<GlobalExceptionMiddleware>();

            if (ConfigurationManager.AppSettings["logRequest"] == "true")
            {
                app.UseMiddlewareFromContainer<LogRequestMiddleware>();
            }
            
            app.UseMiddlewareFromContainer<RequestMiddleware>();
        }
    }
}