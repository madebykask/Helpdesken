using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.TaskScheduler.DI;
using Ninject;
using Ninject.Syntax;
using Quartz;

namespace DH.Helpdesk.TaskScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (var kernel = new StandardKernel(new ServiceModule() ,new QuartzNinjectModule()))
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new TaskScheduler(kernel)
                };
                ServiceBase.Run(ServicesToRun);

            }
        }
    }

}
