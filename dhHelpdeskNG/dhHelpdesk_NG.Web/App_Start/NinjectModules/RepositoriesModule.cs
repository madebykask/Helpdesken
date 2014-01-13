namespace dhHelpdesk_NG.Web.App_Start.NinjectModules
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Data.Repositories.Changes.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete;

    public sealed class RepositoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotifierRepository>().To<NotifierRepository>();
            this.Bind<INotifierFieldSettingRepository>().To<NotifierFieldSettingRepository>();
            this.Bind<INotifierFieldSettingLanguageRepository>().To<NotifierFieldSettingLanguageRepository>();
            this.Bind<INotifierGroupRepository>().To<NotifierGroupRepository>();

            this.Bind<IChangeFieldSettingRepository>().To<ChangeFieldSettingRepository>();
        }
    }
}