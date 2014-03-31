namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Dal.MapperData.Changes;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes.Concrete;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelEventsMailNotifiers;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelRestorers.Changes;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelRestorers.Changes.Concrete;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Changes;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Changes.Concrete;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Common;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Common.Concrete;
    using DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete;

    using Ninject.Modules;

    public sealed class ChangesModule : NinjectModule
    {
        #region Public Methods and Operators

        public override void Load()
        {
            this.Bind<ISearchModelFactory>().To<SearchModelFactory>().InSingletonScope();
            this.Bind<IChangesGridModelFactory>().To<ChangesGridModelFactory>().InSingletonScope();
            this.Bind<ISettingsModelFactory>().To<SettingsModelFactory>().InSingletonScope();
            this.Bind<INewChangeModelFactory>().To<NewChangeModelFactory>().InSingletonScope();
            this.Bind<INewOrdererModelFactory>().To<NewOrdererModelFactory>().InSingletonScope();
            this.Bind<INewGeneralModelFactory>().To<NewGeneralModelFactory>().InSingletonScope();
            this.Bind<INewRegistrationModelFactory>().To<NewRegistrationModelFactory>().InSingletonScope();
            this.Bind<IChangeModelFactory>().To<ChangeModelFactory>().InSingletonScope();
            this.Bind<IOrdererModelFactory>().To<OrdererModelFactory>().InSingletonScope();
            this.Bind<IGeneralModelFactory>().To<GeneralModelFactory>().InSingletonScope();
            this.Bind<IRegistrationModelFactory>().To<RegistrationModelFactory>().InSingletonScope();
            this.Bind<IAnalyzeModelFactory>().To<AnalyzeModelFactory>().InSingletonScope();
            this.Bind<IImplementationModelFactory>().To<ImplementationModelFactory>().InSingletonScope();
            this.Bind<IEvaluationModelFactory>().To<EvaluationModelFactory>().InSingletonScope();
            this.Bind<IHistoriesModelFactory>().To<HistoriesModelFactory>().InSingletonScope();
            this.Bind<ILogsModelFactory>().To<LogsModelFactory>().InSingletonScope();
            this.Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();
            this.Bind<INewChangeRequestFactory>().To<NewChangeRequestFactory>().InSingletonScope();
            this.Bind<IUpdateChangeRequestFactory>().To<UpdateChangeRequestFactory>().InSingletonScope();
            this.Bind<IUpdatedSettingsFactory>().To<UpdatedSettingsFactory>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>>()
                .To<ChangeEntityToChangeDetailedOverviewMapper>();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, Change>>().To<ChangeEntityToChangeMapper>();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>>()
                .To<ChangeFieldSettingsToChangeOverviewSettingsMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>>()
                .To<ChangeFieldSettingsToChangeEditSettingsMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>>()
                .To<ChangeFieldSettingsToFieldSettingsMapper>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>>()
                .To<ChangeFieldSettingsToSearchSettingsMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>>()
                .To<ChangeFieldSettingsToChangeProcessingSettingsMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewChange, ChangeEntity>>()
                .To<NewChangeToChangeEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<UpdatedChange, ChangeHistoryEntity>>()
                .To<ChangeToChangeHistoryEntityMapper>();

            this.Bind<IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>>()
                .To<UpdatedChangeToChangeEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>>()
                .To<UpdatedFieldSettingsToChangeFieldSettingsMapper>()
                .InSingletonScope();

            this.Bind<IChangeLogic>().To<ChangeLogic>().InSingletonScope();
            this.Bind<IHistoriesComparator>().To<HistoriesComparator>().InSingletonScope();
            this.Bind<IElementaryRulesValidator>().To<ElementaryRulesValidator>().InSingletonScope();
            this.Bind<IUpdateChangeRequestValidator>().To<UpdateChangeRequestValidator>().InSingletonScope();
            this.Bind<IChangeRestorer>().To<ChangeRestorer>().InSingletonScope();
            this.Bind<IChangeEmailService>().To<ChangeEmailService>();
            this.Bind<IMailTemplateFormatter<UpdatedChange>>().To<ChangeMailTemplateFormatter>();
            this.Bind<IBusinessModelEventsMailNotifier<UpdateChangeRequest, Change>>().To<ChangeEventsMailNotifier>();
        }

        #endregion
    }
}