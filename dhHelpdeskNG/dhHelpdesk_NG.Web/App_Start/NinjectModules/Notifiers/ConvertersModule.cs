namespace dhHelpdesk_NG.Web.NinjectModules.Notifiers
{
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Notifiers.Concrete;

    using Ninject.Modules;

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