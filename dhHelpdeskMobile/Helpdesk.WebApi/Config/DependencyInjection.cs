using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Helpdesk.WebApi.Config.IdentityServer;
using IdentityServer4.Stores;
using IdentityServer4.Test;

namespace Helpdesk.WebApi.Config
{
    public static class DependencyInjection
    {
        public static void ConfigureDi(IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var cors = new DefaultCorsPolicyService(loggerFactory.CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true
                //AllowedOrigins = { "https://foo", "https://bar" }
            };
            services.AddSingleton<ICorsPolicyService>(cors);
            //TODO: remove -For Tests
            var users = new TestUserStore(Settings.GetUsers());
            services.AddSingleton<TestUserStore>(users);
            /////
            services.AddSingleton<IClientStore, CustomClientStore>();
            //services.AddSingleton<IDistributedCache>();
        }
    }
}
