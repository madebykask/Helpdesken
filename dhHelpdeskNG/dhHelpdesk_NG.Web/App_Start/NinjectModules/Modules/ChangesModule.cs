namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.Dal.Dal.Mappers;
    using DH.Helpdesk.Dal.Dal.Mappers.Changes;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.AggregateDataLoader.Changes;
    using DH.Helpdesk.Services.AggregateDataLoader.Changes.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes.Concrete;
    using DH.Helpdesk.Services.BusinessModelFactories.Changes;
    using DH.Helpdesk.Services.BusinessModelFactories.Changes.Concrete;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete;

    using Ninject.Modules;

    public sealed class ChangesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISearchModelFactory>().To<SearchModelFactory>().InSingletonScope();
            this.Bind<IChangesGridModelFactory>().To<ChangesGridModelFactory>().InSingletonScope();
            this.Bind<ISettingsModelFactory>().To<SettingsModelFactory>().InSingletonScope();
            this.Bind<IChangeModelFactory>().To<ChangeModelFactory>().InSingletonScope();
            this.Bind<INewChangeModelFactory>().To<NewChangeModelFactory>().InSingletonScope();
            this.Bind<ILogsModelFactory>().To<LogsModelFactory>().InSingletonScope();
            this.Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();
            this.Bind<IAnalyzeModelFactory>().To<AnalyzeModelFactory>().InSingletonScope();
            this.Bind<IRegistrationModelFactory>().To<RegistrationModelFactory>().InSingletonScope();
            this.Bind<IImplementationModelFactory>().To<ImplementationModelFactory>().InSingletonScope();
            this.Bind<IEvaluationModelFactory>().To<EvaluationModelFactory>().InSingletonScope();

            this.Bind<IUpdatedChangeFactory>().To<UpdatedChangeFactory>().InSingletonScope();
            this.Bind<IChangeAggregateFactory>().To<ChangeAggregateFactory>().InSingletonScope();
            this.Bind<IUpdatedChangeAggregateFactory>().To<UpdatedChangeAggregateFactory>().InSingletonScope();
            this.Bind<INewChangeAggregateFactory>().To<NewChangeAggregateFactory>().InSingletonScope();
            this.Bind<IUpdatedFieldSettingsFactory>().To<UpdatedFieldSettingsFactory>().InSingletonScope();
            this.Bind<INewChangeFactory>().To<NewChangeFactory>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, Change>>()
                .To<ChangeEntityToChangeMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<Contact, ChangeContactEntity>>()
                .To<ContactToChangeContactEntity>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<Contact, ChangeContactEntity>>()
                .To<ContactToChangeContactEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>>()
                .To<UpdatedChangeToChangeEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>>()
                .To<ChangeEntityToChangeDetailedOverviewMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewChange, ChangeEntity>>()
                .To<NewChangeToChangeEntityMapper>()
                .InSingletonScope();

            this.Bind<IHistoriesComparator>().To<HistoriesComparator>().InSingletonScope();

            this.Bind<IChangeOptionsDataLoader>().To<ChangeOptionsDataLoader>();
            this.Bind<INewChangeOptionsDataLoader>().To<NewChangeOptionsDataLoader>();
            this.Bind<IChangeAggregateDataLoader>().To<ChangeAggregateDataLoader>();

            this.Bind<IChangeLogic>().To<ChangeLogic>().InSingletonScope();
        }
    }
}