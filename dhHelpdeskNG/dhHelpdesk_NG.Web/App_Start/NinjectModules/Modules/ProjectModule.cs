namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Dal.Mappers.Projects;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects.Concrete;

    using Ninject.Modules;

    public class ProjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INewBusinessModelToEntityMapper<NewProject, Project>>()
                .To<NewProjectToProjectEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>()
                .To<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>()
                .To<NewProjectFileToProjectFileEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>()
                .To<NewProjectScheduleToProjectScheduleEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>()
                .To<NewProjectLogToProjectLogEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<UpdatedProject, Project>>()
                .To<UpdatedProjectToProjectEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>()
                .To<UpdatedProjectScheduleToProjectScheduleEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
                .To<ProjectEntityToProjectOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
                .To<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
                .To<ProjectFileEntityToProjectFileOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
                .To<ProjectLogEntityToProjectLogOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
                .To<ProjectScheduleEntityToProjectScheduleOverviewMapper>()
                .InSingletonScope();

            this.Bind<INewProjectViewModelFactory>().To<NewProjectViewModelFactory>().InSingletonScope();
            this.Bind<IUpdatedProjectViewModelFactory>().To<UpdatedProjectViewModelFactory>().InSingletonScope();
        }
    }
}