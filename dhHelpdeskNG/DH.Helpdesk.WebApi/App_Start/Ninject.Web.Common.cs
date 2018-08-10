////using DH.Helpdesk.WebApi;
////using Ninject.Web.Common.WebHost;
////using System;
////using System.Web;
////using Microsoft.Web.Infrastructure.DynamicModuleHelper;
////using DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection;
////using DH.Helpdesk.Services.Services;
////using DH.Helpdesk.Services.Infrastructure;
////using DH.Helpdesk.Dal.Infrastructure;

//////[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DH.Helpdesk.WebApi.App_Start.NinjectWebCommon), "Start")]
//////[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DH.Helpdesk.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.WebApi.App_Start
{


////    public static class NinjectWebCommon 
////    {
////        //private static readonly Bootstrapper bootstrapper = new Bootstrapper();

////        ///// <summary>
////        ///// Starts the application
////        ///// </summary>
////        //public static void Start() 
////        //{
////        //    DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
////        //    DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
////        //    bootstrapper.Initialize(CreateKernel);
////        //}
        
////        ///// <summary>
////        ///// Stops the application.
////        ///// </summary>
////        //public static void Stop()
////        //{
////        //    bootstrapper.ShutDown();
////        //}
        
////        /// <summary>
////        /// Creates the kernel that will manage your application.
////        /// </summary>
////        /// <returns>The created kernel.</returns>
////        public static IKernel CreateKernel()
////        {
////            var kernel = new StandardKernel(
////                new LoggerModule(),
////                new WorkContextModule(),
////                new CommonModule(),
////                new RepositoriesModule(),
////                new ServicesModule()
////                );
////            ManualDependencyResolver.SetKernel(kernel);
////            try
////            {
////                //kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
////                //kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
////                RegisterServices(kernel);
////                //System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
////                return kernel;
////            }
////            catch
////            {
////                kernel.Dispose();
////                throw;
////            }
////        }

////        /// <summary>
////        /// Load your modules or register your services here!
////        /// </summary>
////        /// <param name="kernel">The kernel.</param>
////        private static void RegisterServices(IKernel kernel)
////        {
////        }        
////    }
}