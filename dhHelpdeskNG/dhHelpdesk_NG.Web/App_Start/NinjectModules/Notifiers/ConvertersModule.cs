namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Notifiers
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers.Concrete;

    public sealed class ConvertersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISettingsInputModelToUpdatedFieldsSettingsDtoConverter>()
                .To<SettingsInputModelToUpdatedFieldsSettingsDtoConverter>()
                .InSingletonScope();
        }
    }
}