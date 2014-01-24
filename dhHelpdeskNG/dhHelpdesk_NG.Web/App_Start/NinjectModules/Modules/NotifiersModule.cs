namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Service.Validators.Notifier;
    using dhHelpdesk_NG.Service.Validators.Notifier.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Notifiers.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete;

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

            this.Bind<IUpdatedFieldSettingsFactory>().To<UpdatedFieldSettingsFactory>().InSingletonScope();
            this.Bind<INotifierDynamicRulesValidator>().To<NotifierDynamicRulesValidator>().InSingletonScope();
        }
    }
}