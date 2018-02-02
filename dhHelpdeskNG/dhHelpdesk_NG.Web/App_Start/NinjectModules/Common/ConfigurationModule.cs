using Ninject;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

    using Ninject.Modules;

    internal sealed class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAdfsConfiguration, IAdfsClaimsSettings>().To<AdfsConfiguration>();
            this.Bind<IApplicationConfiguration>().To<ApplicationConfiguration>();
            this.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
        }
    }
}