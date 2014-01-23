namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Dal.Mappers.Problems;
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    using Ninject.Modules;

    public sealed class ProblemModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<INewBusinessModelToEntityMapper<NewProblemDto, Problem>>()
                .To<NewProblemToProblemEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<NewProblemDto, Problem>>()
                .To<ProblemEntityFromBusinessModelChanger>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Problem, ProblemOverview>>()
                .To<ProblemEntityToProblemOverviewMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>()
                .To<NewProblemLogToProblemLogEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>()
                .To<ProblemLogEntityFromBusinessModelChanger>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview>>()
                .To<ProblemLogEntityToProblemLogOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto>>()
                .To<ProblemLogEntityToNewProblemLogMapper>()
                .InSingletonScope();
        }
    }
}