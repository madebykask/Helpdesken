namespace DH.Helpdesk.SelfService
{
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.SelfService.Infrastructure.WorkContext.Concrete;

    using Ninject.Modules;
    using Ninject.Web.Common;

    public sealed class WorkContextModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorkContext>().To<WorkContext>().InRequestScope();
            Bind<IUserContext>().To<UserContext>().InRequestScope();
        }
    }
}