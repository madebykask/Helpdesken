using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Concrete;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using Ninject.Modules;

namespace DH.Helpdesk.TaskScheduler.DI.Modules
{
    public class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            // configuration
            Bind<IApplicationSettings, IGDPRJobSettings>().To<ApplicationSettingsProvider>().InSingletonScope();
            Bind<IServiceConfigurationManager>().To<ServiceConfigurationManager>().InSingletonScope();
            
            Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            Bind<IJsonSerializeService>().To<JsonSerializeService>().InSingletonScope();
        }
    }
}