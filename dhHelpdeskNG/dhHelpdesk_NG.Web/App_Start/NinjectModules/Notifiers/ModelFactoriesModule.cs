namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Notifiers
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete;

    public sealed class ModelFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IIndexModelFactory>().To<IndexModelFactory>().InSingletonScope();
            this.Bind<INotifiersModelFactory>().To<NotifiersModelFactory>().InSingletonScope();
            this.Bind<INotifiersGridModelFactory>().To<NotifiersGridModelFactory>().InSingletonScope();
            this.Bind<INewNotifierModelFactory>().To<NewNotifierModelFactory>().InSingletonScope();
            this.Bind<INotifierModelFactory>().To<NotifierModelFactory>().InSingletonScope();
            this.Bind<INotifierInputFieldModelFactory>().To<NotifierInputFieldModelFactory>().InSingletonScope();
        }
    }
}