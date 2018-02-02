using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class NinjectSchedulerFactory : StdSchedulerFactory
    {
        private readonly NinjectJobFactory _ninjectJobFactory;

        public NinjectSchedulerFactory(NinjectJobFactory ninjectJobFactory)
        {
            _ninjectJobFactory = ninjectJobFactory;
        }

        protected override IScheduler Instantiate(Quartz.Core.QuartzSchedulerResources rsrcs, Quartz.Core.QuartzScheduler qs)
        {
            qs.JobFactory = _ninjectJobFactory;
            return base.Instantiate(rsrcs, qs);
        }
    }
}
