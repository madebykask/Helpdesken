using DH.Helpdesk.TaskScheduler.Jobs;
using DH.Helpdesk.TaskScheduler.Jobs.DailReport;
using DH.Helpdesk.TaskScheduler.Jobs.Gdpr;
using DH.Helpdesk.TaskScheduler.Jobs.Import;
using Ninject.Modules;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.DI.Modules
{
    public class JobsModule : NinjectModule
    {
        public override void Load()
        {
            //TODO: scan assambly for jobs
            Bind<IJob>().To<ImportInitiatorJob>();
            Bind<IJob>().To<DailyReportJob>();
            Bind<IJob>().To<GdprDataPrivacyJob>();

            //initializers
            Bind<IJobInitializer>().To<DailyReportJobInitializer>();
            Bind<IJobInitializer>().To<ImportJobInitializer>();
            Bind<IJobInitializer>().To<GdprJobInitializer>();
        }
    }
}