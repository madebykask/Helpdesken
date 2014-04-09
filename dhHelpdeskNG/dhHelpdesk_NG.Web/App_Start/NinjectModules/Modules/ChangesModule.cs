namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
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
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Changes.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.Changes;
    using DH.Helpdesk.Services.BusinessLogic.Changes.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared.Concrete;
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
            this.Bind<INewLogModelFactory>().To<NewLogModelFactory>().InSingletonScope();
            this.Bind<IChangeModelFactory>().To<ChangeModelFactory>().InSingletonScope();
            this.Bind<IOrdererModelFactory>().To<OrdererModelFactory>().InSingletonScope();
            this.Bind<IGeneralModelFactory>().To<GeneralModelFactory>().InSingletonScope();
            this.Bind<IRegistrationModelFactory>().To<RegistrationModelFactory>().InSingletonScope();
            this.Bind<IAnalyzeModelFactory>().To<AnalyzeModelFactory>().InSingletonScope();
            this.Bind<IImplementationModelFactory>().To<ImplementationModelFactory>().InSingletonScope();
            this.Bind<IEvaluationModelFactory>().To<EvaluationModelFactory>().InSingletonScope();
            this.Bind<ILogModelFactory>().To<LogModelFactory>().InSingletonScope();
            this.Bind<IHistoriesModelFactory>().To<HistoriesModelFactory>().InSingletonScope();
            this.Bind<ILogsModelFactory>().To<LogsModelFactory>().InSingletonScope();
            this.Bind<IConfigurableFieldModelFactory>().To<ConfigurableFieldModelFactory>().InSingletonScope();
            this.Bind<INewChangeRequestFactory>().To<NewChangeRequestFactory>().InSingletonScope();
            this.Bind<IUpdateChangeRequestFactory>().To<UpdateChangeRequestFactory>().InSingletonScope();
            this.Bind<IUpdatedSettingsFactory>().To<UpdatedSettingsFactory>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>>()
                .To<ChangeEntityToChangeDetailedOverviewMapper>();

            this.Bind<IEntityToBusinessModelMapper<ChangeEntity, Change>>().To<ChangeEntityToChangeMapper>();

            this
                .Bind
                <
                    IEntityToBusinessModelMapper
                        <NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>>()
                .To<ChangeFieldSettingsToChangeOverviewSettingsMapper>()
                .InSingletonScope();

            this
                .Bind
                <IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>>()
                .To<ChangeFieldSettingsToChangeEditSettingsMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>>(
                ).To<ChangeFieldSettingsToFieldSettingsMapper>().InSingletonScope();

            this
                .Bind
                <IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>>()
                .To<ChangeFieldSettingsToSearchSettingsMapper>()
                .InSingletonScope();

            this
                .Bind
                <
                    IEntityToBusinessModelMapper
                        <NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>>()
                .To<ChangeFieldSettingsToChangeProcessingSettingsMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewChange, ChangeEntity>>()
                .To<NewChangeToChangeEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<History, ChangeHistoryEntity>>()
                .To<HistoryToChangeHistoryEntityMapper>();

            this.Bind<IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>>()
                .To<UpdatedChangeToChangeEntityMapper>();

            this
                .Bind
                <IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>>()
                .To<UpdatedFieldSettingsToChangeFieldSettingsMapper>()
                .InSingletonScope();

            this.Bind<IChangeLogic>().To<ChangeLogic>().InSingletonScope();
            this.Bind<IHistoriesComparator>().To<HistoriesComparator>().InSingletonScope();
            this.Bind<IElementaryRulesValidator>().To<ElementaryRulesValidator>().InSingletonScope();
            this.Bind<IUpdateChangeRequestValidator>().To<UpdateChangeRequestValidator>().InSingletonScope();
            this.Bind<IChangeRestorer>().To<ChangeRestorer>().InSingletonScope();
            this.Bind<IChangeEmailService>().To<ChangeEmailService>();
            this.Bind<IMailTemplateFormatter<UpdatedChange>>().To<ChangeMailTemplateFormatter>();

            this.Bind<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditOptionalData>>()
                .To<SimpleNotificationsAudit>();

            this.Bind<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditOptionalData>>().To<InvitationToCabAudit>();
            this.Bind<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditOptionalData>>().To<OwnerChangedAuditor>();
            this.Bind<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditOptionalData>>().To<StatusChangedAuditor>();

            this.Bind<IBusinessModelsMapper<UpdateChangeRequest, History>>()
                .To<ChangeToChangeHistoryMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>>>()
                .To<OverviewSettingsToExcelSettingsMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem>>()
                .To<ChangeDetailedOverviewToBusinessItemsMapper>()
                .InSingletonScope();
        }

        #endregion
    }
}