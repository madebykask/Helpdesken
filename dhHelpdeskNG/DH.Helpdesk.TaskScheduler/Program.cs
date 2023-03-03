using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using DH.Helpdesk.TaskScheduler.DI;
using DH.Helpdesk.TaskScheduler.DI.Modules;
using Ninject;
using Ninject.Modules;

namespace DH.Helpdesk.TaskScheduler
{
    static class Program
    {

        public static int ProcessedTaskId = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Debugger.Launch();
            var modules = new List<NinjectModule>
            {
                new InfrastructureModule(),
                new DatabaseModule(),
                new ServiceModule(),
                new JobsModule(),
                new QuartzNinjectModule()
            };

            using (var kernel = new StandardKernel(modules.Cast<INinjectModule>().ToArray()))
            {
                
                var servicesToRun = new ServiceBase[]
                {
                    new TaskScheduler(kernel)
                };

                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
