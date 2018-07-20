//using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.WebApi.Infrastructure.Contexts;
using Ninject.Modules;
using Ninject.Web.Common;
//    using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{


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
            //Bind<ICacheContext>().To<CacheContext>().InRequestScope();
            //Bind<ICustomerContext>().To<CustomerContext>().InRequestScope();
            //Bind<ISessionContext>().To<SessionContext>().InRequestScope();
            //Bind<IApplicationContext>().To<ApplicationContext>().InRequestScope();

        }
    }
}