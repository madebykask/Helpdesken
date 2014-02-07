namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Dal.Mappers;
    using DH.Helpdesk.Dal.Dal.Mappers.Problems;
    using DH.Helpdesk.Domain.Problems;

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