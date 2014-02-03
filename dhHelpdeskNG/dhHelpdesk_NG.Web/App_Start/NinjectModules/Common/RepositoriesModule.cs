namespace dhHelpdesk_NG.Web.NinjectModules.Common
{
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Data.Repositories.Changes.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.Data.Repositories.Projects.Concrete;

    using Ninject.Modules;

    public sealed class RepositoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INotifierRepository>().To<NotifierRepository>();
            this.Bind<INotifierFieldSettingRepository>().To<NotifierFieldSettingRepository>();
            this.Bind<INotifierFieldSettingLanguageRepository>().To<NotifierFieldSettingLanguageRepository>();
            this.Bind<INotifierGroupRepository>().To<NotifierGroupRepository>();

            this.Bind<IChangeFieldSettingRepository>().To<ChangeFieldSettingRepository>();
            this.Bind<IChangeContactRepository>().To<ChangeContactRepository>();
            this.Bind<IChangeHistoryRepository>().To<ChangeHistoryRepository>();
            this.Bind<IChangeChangeRepository>().To<ChangeChangeRepository>();

            this.Bind<IProjectRepository>().To<ProjectRepository>();
            this.Bind<IProjectScheduleRepository>().To<ProjectScheduleRepository>();
            this.Bind<IProjectCollaboratorRepository>().To<ProjectCollaboratorRepository>();
            this.Bind<IProjectFileRepository>().To<ProjectFileRepository>();
            this.Bind<IProjectLogRepository>().To<ProjectLogRepository>();

            this.Bind<IEmailGroupEmailRepository>().To<EmailGroupEmailRepository>();
        }
    }
}