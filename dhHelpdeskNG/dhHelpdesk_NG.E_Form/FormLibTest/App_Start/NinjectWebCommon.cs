[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FormLibTest.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FormLibTest.App_Start.NinjectWebCommon), "Stop")]

namespace FormLibTest.App_Start
{
    using System;
    using System.Configuration;
    using System.Web;
    using DH.Helpdesk.EForm.Core.Service;
    using DH.Helpdesk.EForm.FormLib;
    using DH.Helpdesk.EForm.Model.Abstract;
    using DH.Helpdesk.EForm.Model.Contrete;
    using DH.Helpdesk.EForm.Service;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DSN"].ConnectionString;
            
            kernel.Bind<IGlobalViewRepository>().To<GlobalViewRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);
            kernel.Bind<IContractRepository>().To<ContractRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);
            //kernel.Bind<ITextRepository>().To<TextRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);
            kernel.Bind<IFileService>().To<FileService>().InRequestScope();
        }        
    }
}
