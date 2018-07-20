using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.WebApi.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Ninject.Web.WebApi;
using Owin;

[assembly: OwinStartup(typeof(DH.Helpdesk.WebApi.Startup))]
namespace DH.Helpdesk.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ConfigureAuth(app);
            
            WebApiConfig.Register(config);    
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}