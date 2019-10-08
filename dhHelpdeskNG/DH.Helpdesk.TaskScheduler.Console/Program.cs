using DH.Helpdesk.TaskScheduler.DI.Modules;
using DH.Helpdesk.TaskScheduler.Jobs.Import;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var modules = new List<NinjectModule>
			{
				new InfrastructureModule(),
				new DatabaseModule(),
				new ServiceModule(),
				new JobsModule(),
				new QuartzNinjectModule()
			};
			var settings = new NinjectSettings
			{
				InjectNonPublic = true
			};
			var kernel = new StandardKernel(settings, modules.Cast<INinjectModule>().ToArray());
			
			var scheduler = new ConsoleTaskScheduler(kernel);

			scheduler.Start();

			//var method = scheduler.GetType().GetMethod("OnStart", System.Reflection.BindingFlags.NonPublic);

			//method.Invoke(scheduler, null);
			
		}
	}

	public class ConsoleTaskScheduler : TaskScheduler
	{
		public ConsoleTaskScheduler(IKernel kernel) : base(kernel)
		{

		}

		public void Start()
		{
			base.OnStart(new string[0]);
		}
	}
}
