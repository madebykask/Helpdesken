namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Dal.Mappers.Projects;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    using Ninject.Modules;

    public class ProjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INewBusinessModelToEntityMapper<NewProjectDto, Project>>()
                .To<ProjectBusinessModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectCollaboratorDto, ProjectCollaborator>>()
                .To<ProjectCollaboratorBusinessModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectFileDto, ProjectFile>>()
                .To<ProjectFileBusinessModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectScheduleDto, ProjectSchedule>>()
                .To<ProjectScheduleBusinessModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectLogDto, ProjectLog>>()
                .To<ProjectLogBusinessModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<NewProjectDto, Project>>()
                .To<ProjectEntityFromBusinessModelChanger>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<NewProjectScheduleDto, ProjectSchedule>>()
                .To<ProjectScheduleEntityFromBusinessModelChanger>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
                .To<ProjectEntityToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
                .To<ProjectCollaboratorEntityToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
                .To<ProjectFileEntityToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
                .To<ProjectLogEntityToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
                .To<ProjectScheduleEntityToBusinessModelMapper>()
                .InSingletonScope();
        }
    }
}