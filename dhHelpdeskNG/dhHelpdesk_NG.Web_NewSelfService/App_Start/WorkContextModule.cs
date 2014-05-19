namespace DH.Helpdesk.NewSelfService
{
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.NewSelfService.Infrastructure.WorkContext.Concrete;

    using Ninject.Modules;
    using Ninject.Web.Common;

    public sealed class WorkContextModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IWorkContext>().To<WorkContext>().InRequestScope();
            this.Bind<IUserContext>().To<UserContext>().InRequestScope();
        }
    }
}