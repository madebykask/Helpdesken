namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.Services.Restorers;
    using DH.Helpdesk.Services.Restorers.Notifiers;
    using DH.Helpdesk.Services.Validators.Notifiers;
    using DH.Helpdesk.Services.Validators.Notifiers.Concrete;
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

            this.Bind<INewNotifierFactory>().To<NewNotifierFactory>().InSingletonScope();
            this.Bind<IUpdatedNotifierFactory>().To<UpdatedNotifierFactory>().InSingletonScope();
            this.Bind<IUpdatedSettingsFactory>().To<UpdatedSettingsFactory>().InSingletonScope();

            this.Bind<IRestorer<Notifier, NotifierProcessingSettings>>().To<NotifierRestorer>().InSingletonScope();
            this.Bind<INotifierDynamicRulesValidator>().To<NotifierElementaryRulesValidator>().InSingletonScope();
        }
    }
}