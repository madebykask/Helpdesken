namespace dhHelpdesk_NG.Web.NinjectModules.Changes
{
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete;

    using Ninject.Modules;

    public sealed class ModelFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISettingsModelFactory>().To<SettingsModelFactory>().InSingletonScope();
            this.Bind<IChangesGridModelFactory>().To<ChangesGridModelFactory>().InSingletonScope();
            this.Bind<ISearchModelFactory>().To<SearchModelFactory>().InSingletonScope();
        }
    }
}