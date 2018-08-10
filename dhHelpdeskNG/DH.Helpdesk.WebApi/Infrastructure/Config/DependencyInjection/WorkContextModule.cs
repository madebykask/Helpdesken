//using DH.Helpdesk.Web.Infrastructure.Authentication;

using Autofac;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.WebApi.Infrastructure.Contexts;
//    using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{


    /// <summary>
    /// The work context module.
    /// </summary>
    public sealed class WorkContextModule : Module
    {
        /// <summary>
        /// The load.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();
            builder.RegisterType<UserContext>().As<IUserContext>().InstancePerRequest();
            //Bind<ICacheContext>().To<CacheContext>().InRequestScope();
            //Bind<ICustomerContext>().To<CustomerContext>().InRequestScope();
            //Bind<ISessionContext>().To<SessionContext>().InRequestScope();
            //Bind<IApplicationContext>().To<ApplicationContext>().InRequestScope();

        }
    }
}