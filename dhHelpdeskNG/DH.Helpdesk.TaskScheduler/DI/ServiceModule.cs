using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.TaskScheduler.Jobs;
using DH.Helpdesk.TaskScheduler.Managers;
using DH.Helpdesk.TaskScheduler.Services;
using Ninject.Extensions.Conventions.Extensions;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceConfigurationManager>().To<ServiceConfigurationManager>().InSingletonScope();

            Bind<IDbQueryExecutor>().To<SqlDbQueryExecutor>();
            Bind<IDbQueryExecutorFactory>().To<SqlDbQueryExecutorFactory>();
            Bind<IDailyReportService>().To<DailyReportService>();

            Bind<IJob>().To<DailyReportJob>();//TODO: scan assambly for jobs
        }
    }
}



