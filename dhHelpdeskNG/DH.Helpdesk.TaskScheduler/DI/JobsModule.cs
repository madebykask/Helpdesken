using DH.Helpdesk.TaskScheduler.Jobs;
using Ninject.Modules;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class JobsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJob>().To<ImportInitiatorJob>();
            Bind<IJob>().To<DailyReportJob>();//TODO: scan assambly for jobs
        }
    }
}