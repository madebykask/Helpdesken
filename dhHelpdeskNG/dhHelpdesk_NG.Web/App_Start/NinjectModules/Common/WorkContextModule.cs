// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkContextModule.cs" company="">
//   
// </copyright>
// <summary>
//   The work context module.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.Web.Infrastructure.Authentication;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

    using Ninject.Modules;
    using Ninject.Web.Common;

    /// <summary>
    /// The work context module.
    /// </summary>
    public sealed class WorkContextModule : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            Bind<IWorkContext>().To<WorkContext>().InRequestScope();
            Bind<IUserContext>().To<UserContext>().InRequestScope();
            Bind<ICacheContext>().To<CacheContext>().InRequestScope();
            Bind<ICustomerContext>().To<CustomerContext>().InRequestScope();
            Bind<ISessionContext>().To<SessionContext>().InRequestScope();
            Bind<IApplicationContext>().To<ApplicationContext>().InRequestScope();

        }
    }
}