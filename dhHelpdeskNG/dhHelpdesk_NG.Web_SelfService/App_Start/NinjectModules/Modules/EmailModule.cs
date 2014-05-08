namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email.Concrete;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.Services.Infrastructure.Email.Concrete;
    using Ninject.Modules;
    using Ninject.Web.Common;
    internal sealed class EmailModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEmailFactory>().To<EmailFactory>().InSingletonScope();

            this.Bind<ICaseMailer>().To<CaseMailer>().InRequestScope();
        }
    }
}