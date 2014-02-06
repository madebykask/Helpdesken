namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Dal.Mappers.Changes;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeDetailedOverview;
    using dhHelpdesk_NG.Service.BusinessLogic.Changes;
    using dhHelpdesk_NG.Service.BusinessLogic.Changes.Concrete;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes;
    using dhHelpdesk_NG.Service.BusinessModelFactories.Changes.Concrete;
    using dhHelpdesk_NG.Service.Loaders.Changes;
    using dhHelpdesk_NG.Service.Loaders.Changes.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete;

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
            this.Bind<IAnalyzeModelFactory>().To<AnalyzeModelFactory>().InSingletonScope();
            this.Bind<IRegistrationModelFactory>().To<RegistrationModelFactory>().InSingletonScope();
            this.Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();

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
            this.Bind<IChangeOptionalDataLoader>().To<ChangeOptionalDataLoader>();
        }
    }
}