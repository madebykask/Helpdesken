namespace DH.Helpdesk.Mobile.NinjectModules.Common
{
    using DH.Helpdesk.Mobile.Infrastructure.Configuration;
    using DH.Helpdesk.Mobile.Infrastructure.Configuration.Concrete;

    using Ninject.Modules;

    internal sealed class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IApplicationConfiguration>().To<ApplicationConfiguration>().InSingletonScope();
            this.Bind<IConfiguration>().To<Configuration>().InSingletonScope();
        }
    }
}