namespace dhHelpdesk_NG.Web.App_Start.NinjectModules
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete;

    public sealed class RepositoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotifierFieldSettingLanguageRepository>().To<NotifierFieldSettingLanguageRepository>();
        }
    }
}