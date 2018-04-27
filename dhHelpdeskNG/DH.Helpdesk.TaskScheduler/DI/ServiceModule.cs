using DH.Helpdesk.Services.Services;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using DH.Helpdesk.TaskScheduler.Jobs.Gdpr;
using Ninject.Modules;
using DH.Helpdesk.TaskScheduler.Services;

using DailyReportService = DH.Helpdesk.TaskScheduler.Services.DailyReportService;
using IDailyReportService = DH.Helpdesk.TaskScheduler.Services.IDailyReportService;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // configuration
            Bind<IApplicationSettings>().To<ApplicationSettingsProvider>().InSingletonScope();
            Bind<IServiceConfigurationManager>().To<ServiceConfigurationManager>().InSingletonScope();

            // services
            Bind<IGDPRTasksService>().To<GDPRTasksService>();
            Bind<IDailyReportService>().To<DailyReportService>();
            Bind<IImportInitiatorService>().To<ImportInitiatorService>();
            Bind<IOUService>().To<OUService>();
        }
    }
}



