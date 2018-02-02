using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Quartz;
using Ninject;

namespace DH.Helpdesk.TaskScheduler.DI
{
	public class QuartzNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
			Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();   
		}
	}
}

