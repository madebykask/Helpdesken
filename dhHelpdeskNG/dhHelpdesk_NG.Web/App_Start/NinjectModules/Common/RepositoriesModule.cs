namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Dal.Repositories.Changes.Concrete;
    using DH.Helpdesk.Dal.Repositories.Concrete;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Dal.Repositories.Notifiers.Concrete;
    using DH.Helpdesk.Dal.Repositories.Projects;
    using DH.Helpdesk.Dal.Repositories.Projects.Concrete;

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