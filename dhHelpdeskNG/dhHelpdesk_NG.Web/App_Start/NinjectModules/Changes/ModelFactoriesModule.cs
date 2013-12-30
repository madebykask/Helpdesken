namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Changes
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete;

    public sealed class ModelFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISettingsModelFactory>().To<SettingsModelFactory>().InSingletonScope();
        }
    }
}