using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.TaskScheduler.Services;
using Ninject.Modules;
using DailyReportService = DH.Helpdesk.TaskScheduler.Services.DailyReportService;
using IDailyReportService = DH.Helpdesk.TaskScheduler.Services.IDailyReportService;

namespace DH.Helpdesk.TaskScheduler.DI.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // services
            Bind<IDailyReportService>().To<DailyReportService>();
            Bind<IImportInitiatorService>().To<ImportInitiatorService>();
            Bind<IOUService>().To<OUService>();
            Bind<IGDPRTasksService>().To<GDPRTasksService>();
            Bind<IGDPRDataPrivacyProcessor>().To<GDPRDataPrivacyProcessor>();
            Bind<ISettingsLogic>().To<SettingsLogic>();
        }
    }
}