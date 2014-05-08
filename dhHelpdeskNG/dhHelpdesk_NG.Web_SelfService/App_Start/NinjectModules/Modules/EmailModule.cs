namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email.Concrete;

    using Ninject.Modules;

    internal sealed class EmailModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEmailFactory>().To<EmailFactory>().InSingletonScope();
        }
    }
}