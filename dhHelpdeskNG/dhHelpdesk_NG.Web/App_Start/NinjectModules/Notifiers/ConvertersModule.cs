namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Notifiers
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Notifiers.Concrete;

    public sealed class ConvertersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUpdatedFieldSettingsFactory>()
                .To<UpdatedFieldSettingsFactory>()
                .InSingletonScope();
        }
    }
}