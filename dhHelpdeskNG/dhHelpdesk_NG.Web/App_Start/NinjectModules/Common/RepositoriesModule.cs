using DH.Helpdesk.Dal.Repositories.Questionnaire;
using DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete;

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

            this.Bind<IChangeRepository>().To<ChangeRepository>();
            this.Bind<IChangeStatusRepository>().To<ChangeStatusRepository>();
            this.Bind<IChangeObjectRepository>().To<ChangeObjectRepository>();
            this.Bind<IChangeGroupRepository>().To<ChangeGroupRepository>();
            this.Bind<IChangeFileRepository>().To<ChangeFileRepository>();
            this.Bind<IChangeLogRepository>().To<ChangeLogRepository>();
            this.Bind<IChangeCategoryRepository>().To<ChangeCategoryRepository>();
            this.Bind<IChangeChangeRepository>().To<ChangeChangeRepository>();
            this.Bind<IChangePriorityRepository>().To<ChangePriorityRepository>();
            this.Bind<IChangeImplementationStatusRepository>().To<ChangeImplementationStatusRepository>();
            this.Bind<IChangeHistoryRepository>().To<ChangeHistoryRepository>();
            this.Bind<IChangeEmailLogRepository>().To<ChangeEmailLogRepository>();
            this.Bind<IChangeFieldSettingRepository>().To<ChangeFieldSettingRepository>();
            this.Bind<IChangeChangeGroupRepository>().To<ChangeChangeGroupRepository>();
            this.Bind<IChangeDepartmentRepository>().To<ChangeDepartmentRepository>();

            this.Bind<IProjectRepository>().To<ProjectRepository>();
            this.Bind<IProjectScheduleRepository>().To<ProjectScheduleRepository>();
            this.Bind<IProjectCollaboratorRepository>().To<ProjectCollaboratorRepository>();
            this.Bind<IProjectFileRepository>().To<ProjectFileRepository>();
            this.Bind<IProjectLogRepository>().To<ProjectLogRepository>();

            this.Bind<IQuestionnaireRepository>().To<QuestionnaireRepository>();
            this.Bind<IQuestionnaireQuestionRepository>().To<QuestionnaireQuestionRepository>();

            this.Bind<IEmailGroupEmailRepository>().To<EmailGroupEmailRepository>();
        }
    }
}