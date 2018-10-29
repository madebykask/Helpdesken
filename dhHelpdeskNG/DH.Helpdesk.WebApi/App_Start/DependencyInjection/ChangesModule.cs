using Autofac;
using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel;
using DH.Helpdesk.Domain.Changes;


namespace DH.Helpdesk.WebApi.DependencyInjection
{
    public class ChangesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //builder.RegisterType<SearchModelFactory>().As<ISearchModelFactory>().SingleInstance();
            //builder.RegisterType<ChangesGridModelFactory>().As<IChangesGridModelFactory>().SingleInstance();
            //builder.RegisterType<SettingsModelFactory>().As<ISettingsModelFactory>().SingleInstance();

            //builder.RegisterType<NewChangeModelFactory>().As<INewChangeModelFactory>().SingleInstance();
            //builder.RegisterType<NewOrdererModelFactory>().As<INewOrdererModelFactory>().SingleInstance();
            //builder.RegisterType<NewGeneralModelFactory>().As<INewGeneralModelFactory>().SingleInstance();
            //builder.RegisterType<NewRegistrationModelFactory>().As<INewRegistrationModelFactory>().SingleInstance();
            //builder.RegisterType<NewLogModelFactory>().As<INewLogModelFactory>().SingleInstance();

            //builder.RegisterType<ChangeModelFactory>().As<IChangeModelFactory>().SingleInstance();
            //builder.RegisterType<OrdererModelFactory>().As<IOrdererModelFactory>().SingleInstance();
            //builder.RegisterType<GeneralModelFactory>().As<IGeneralModelFactory>().SingleInstance();
            //builder.RegisterType<RegistrationModelFactory>().As<IRegistrationModelFactory>().SingleInstance();
            //builder.RegisterType<AnalyzeModelFactory>().As<IAnalyzeModelFactory>().SingleInstance();
            //builder.RegisterType<ImplementationModelFactory>().As<IImplementationModelFactory>().SingleInstance();
            //builder.RegisterType<EvaluationModelFactory>().As<IEvaluationModelFactory>().SingleInstance();
            //builder.RegisterType<LogModelFactory>().As<ILogModelFactory>().SingleInstance();
            //builder.RegisterType<HistoryModelFactory>().As<IHistoryModelFactory>().SingleInstance();

            //builder.RegisterType<ConfigurableFieldModelFactory>().As<IConfigurableFieldModelFactory>().SingleInstance();
            //builder.RegisterType<LogsModelFactory>().As<ILogsModelFactory>().SingleInstance();

            //builder.RegisterType<NewChangeRequestFactory>().As<INewChangeRequestFactory>().SingleInstance();
            //builder.RegisterType<UpdateChangeRequestFactory>().As<IUpdateChangeRequestFactory>().SingleInstance();
            //builder.RegisterType<UpdatedSettingsFactory>().As<IUpdatedSettingsFactory>().SingleInstance();

            //builder.RegisterType<ChangeLogic>().As<IChangeLogic>().SingleInstance();
            //builder.RegisterType<UpdateChangeRequestValidator>().As<IUpdateChangeRequestValidator>().SingleInstance();
            //builder.RegisterType<ChangeRestorer>().As<IChangeRestorer>().SingleInstance();

            //builder.RegisterType<InvitationToCabAudit>().As<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>>();
            //builder.RegisterType<ManualLogsAudit>().As<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>>();
            //builder.RegisterType<OwnerChangedAuditor>().As<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>>();
            //builder.RegisterType<StatusChangedAuditor>().As<IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>>();

            //builder.RegisterType<ChangeDetailedOverviewToBusinessItemMapper>()
            //    .As<IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem>>()
            //    .SingleInstance();

            //builder.RegisterType<ChangeOverviewSettingsToExcelTableHeadersMapper>()
            //    .As<IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>>>()
            //    .SingleInstance();

            //builder.RegisterType<UpdateChangeRequestToHistoryMapper>()
            //    .As<IBusinessModelsMapper<UpdateChangeRequest, History>>()
            //    .SingleInstance();

            builder.RegisterType<NewChangeToChangeEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewChange, ChangeEntity>>()
                .SingleInstance();

            //builder.RegisterType<HistoryToChangeHistoryEntityMapper>()
            //    .As<INewBusinessModelToEntityMapper<History, ChangeHistoryEntity>>();

            //builder.RegisterType<UpdatedChangeToChangeEntityMapper>()
            //    .As<IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>>();

            //builder.RegisterType<ChangeFieldSettingsToChangeFieldSettingsEntityMapper>()
            //    .As<IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>>()
            //    .SingleInstance();

            builder.RegisterType<ChangeEntityToChangeDetailedOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>>();

            //builder.RegisterType<ChangeEntityToChangeMapper>()
            //    .As<IEntityToBusinessModelMapper<ChangeEntity, Change>>();

            //builder.RegisterType
            //    <ChangeFieldSettingsToChangeOverviewSettingsMapper>()
            //    .As<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>>()
            //    .SingleInstance();

            //builder.RegisterType<ChangeFieldSettingsToChangeEditSettingsMapper>()
            //    .As<IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>>()
            //    .SingleInstance();

            //builder.RegisterType<ChangeFieldSettingsToFieldSettingsMapper>()
            //    .As<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>>()
            //    .SingleInstance();

            //builder.RegisterType<ChangeFieldSettingsToSearchSettingsMapper>()
            //    .As<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>>()
            //    .SingleInstance();

            //builder.RegisterType<ChangeFieldSettingsToChangeProcessingSettingsMapper>()
            //    .As<IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>>()
            //    .SingleInstance();
        }
    }
}