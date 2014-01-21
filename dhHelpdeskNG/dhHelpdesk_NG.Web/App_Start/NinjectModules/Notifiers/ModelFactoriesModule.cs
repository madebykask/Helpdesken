namespace dhHelpdesk_NG.Web.NinjectModules.Notifiers
{
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete;

    using Ninject.Modules;

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