namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Changes
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes.Concrete;

    public sealed class DtoFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUpdatedFieldSettingsFactory>().To<UpdatedFieldSettingsFactory>().InSingletonScope();
        }
    }
}