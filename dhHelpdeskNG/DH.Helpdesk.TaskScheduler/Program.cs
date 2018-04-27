using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using DH.Helpdesk.TaskScheduler.DI;
using Ninject;
using Ninject.Modules;

namespace DH.Helpdesk.TaskScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var modules = new List<NinjectModule>
            {
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
