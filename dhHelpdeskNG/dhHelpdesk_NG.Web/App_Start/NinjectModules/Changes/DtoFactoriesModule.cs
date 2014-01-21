namespace dhHelpdesk_NG.Web.NinjectModules.Changes
{
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes.Concrete;

    using Ninject.Modules;

    public sealed class DtoFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUpdatedFieldSettingsFactory>().To<UpdatedFieldSettingsFactory>().InSingletonScope();
        }
    }
}