using Ninject;
using Ninject.Modules;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.DI.Modules
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

