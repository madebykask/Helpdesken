namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Projects;
    using DH.Helpdesk.Domain.Projects;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects.Concrete;

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

            this.Bind<INewProjectFactory>().To<NewProjectFactory>().InSingletonScope();
            this.Bind<INewProjectScheduleFactory>().To<NewProjectScheduleFactory>().InSingletonScope();
            this.Bind<INewProjectLogFactory>().To<NewProjectLogFactory>().InSingletonScope();
            this.Bind<IUpdatedProjectFactory>().To<UpdatedProjectFactory>().InSingletonScope();
            this.Bind<IUpdatedProjectScheduleFactory>().To<UpdatedProjectScheduleFactory>().InSingletonScope();
            this.Bind<IIndexProjectViewModelFactory>().To<IndexProjectViewModelFactory>().InSingletonScope();
        }
    }
}