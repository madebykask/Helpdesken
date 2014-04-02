using DH.Helpdesk.Web.Infrastructure.WorkContext;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;
using Ninject.Modules;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    public sealed class WorkContextModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorkContext>().To<WorkContext>().InRequestScope();
            Bind<IUserContext>().To<UserContext>().InRequestScope();
        }
    }
}