using DH.Helpdesk.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.Web
{
    using System;
    using System.Web;

    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Web.NinjectModules.Common;
    using DH.Helpdesk.Web.NinjectModules.Modules;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        #region Static Fields

        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        #endregion

        #region Public Methods and Operators

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        #endregion

        #region Methods

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
                new ChangesModule(),
                new FaqModule(),
                new LinkModule(),
                new NotifiersModule(),
                new ProblemModule(),
                new ProjectModule(),
                new RepositoriesModule(),
                new ServicesModule(),
                new ToolsModule(),
                new UserModule(),
                new WorkContextModule(),
                new CommonModule(),
                new InventoryModule(),
                new CasesModule());

            ManualDependencyResolver.SetKernel(kernel);

            kernel.Bind<Func<IKernel>>().ToMethod(c => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            return kernel;
        }

        #endregion
    }
}