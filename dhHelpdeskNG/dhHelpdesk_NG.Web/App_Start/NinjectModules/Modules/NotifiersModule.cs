namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Services.Validators.Notifier;
    using DH.Helpdesk.Services.Validators.Notifier.Concrete;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete;

    using Ninject.Modules;

    public sealed class NotifiersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IIndexModelFactory>().To<IndexModelFactory>().InSingletonScope();
            this.Bind<INotifiersModelFactory>().To<NotifiersModelFactory>().InSingletonScope();
            this.Bind<INotifiersGridModelFactory>().To<NotifiersGridModelFactory>().InSingletonScope();
            this.Bind<INewNotifierModelFactory>().To<NewNotifierModelFactory>().InSingletonScope();
            this.Bind<INotifierModelFactory>().To<NotifierModelFactory>().InSingletonScope();
            this.Bind<INotifierInputFieldModelFactory>().To<NotifierInputFieldModelFactory>().InSingletonScope();
            this.Bind<ISettingsModelFactory>().To<SettingsModelFactory>().InSingletonScope();

            this.Bind<IUpdatedFieldSettingsFactory>().To<UpdatedFieldSettingsFactory>().InSingletonScope();
            this.Bind<INotifierDynamicRulesValidator>().To<NotifierElementaryRulesValidator>().InSingletonScope();
        }
    }
}