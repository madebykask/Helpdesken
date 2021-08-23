using DH.Helpdesk.Services.Services.Concrete;

namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Computer;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Printer;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Server;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory.Concrete;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete;

    using Ninject.Modules;

    public class InventoryModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit>>().To<ComputerFieldSettingsToComputerEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview>>().To<ComputerFieldSettingsToComputerOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings>>().To<ComputerFieldSettingsToFieldSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter>>().To<ComputerFieldSettingsToSearchSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForShortInfo>>().To<ComputerFieldSettingsToShortInfoSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ComputerFieldsSettingsProcessing>>().To<ComputerSettingsToComputerProcessingSettingsMapper>();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ServerFieldsSettingsForModelEdit>>().To<ServerFieldSettingsToServerEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ServerFieldsSettingsOverview>>().To<ServerFieldSettingsToServerOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ServerFieldsSettings>>().To<ServerFieldSettingsToFieldSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ServerFieldsSettingsProcessing>>().To<ServerFieldSettingsToServerProcessingSettingsMapper>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, PrinterFieldsSettingsForModelEdit>>().To<PrinterFieldSettingsToPrinterEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, PrinterFieldsSettingsOverview>>().To<PrinterFieldSettingsToPrinterOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, PrinterFieldsSettings>>().To<PrinterFieldSettingsToFieldSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, PrinterFieldsSettingsProcessing>>().To<PrinterFieldSettingsToPrinterProcessingSettingsMapper>().InSingletonScope();

            this.Bind<IConfigurableFieldModelBuilder>().To<ConfigurableFieldModelBuilder>().InSingletonScope();
            this.Bind<IComputerViewModelBuilder>().To<ComputerViewModelBuilder>().InSingletonScope();
            this.Bind<IServerViewModelBuilder>().To<ServerViewModelBuilder>().InSingletonScope();
            this.Bind<IPrinterViewModelBuilder>().To<PrinterViewModelBuilder>().InSingletonScope();
            this.Bind<IInventoryViewModelBuilder>().To<InventoryViewModelBuilder>().InSingletonScope();
            this.Bind<IDynamicsFieldsModelBuilder>().To<DynamicsFieldsModelBuilder>().InSingletonScope();

            this.Bind<IFieldSettingBuilder>().To<FieldSettingBuilder>().InSingletonScope();
            this.Bind<IFieldSettingModelBuilder>().To<FieldSettingModelBuilder>().InSingletonScope();

            this.Bind<IComputerFieldsSettingsBuilder>().To<ComputerFieldsSettingsBuilder>().InSingletonScope();
            this.Bind<IServerFieldsSettingsBuilder>().To<ServerFieldsSettingsBuilder>().InSingletonScope();
            this.Bind<IPrinterFieldsSettingsBuilder>().To<PrinterFieldsSettingsBuilder>().InSingletonScope();
            this.Bind<IInventoryFieldsSettingsBuilder>().To<InventoryFieldsSettingsBuilder>().InSingletonScope();

            this.Bind<IComputerFieldsSettingsViewModelBuilder>().To<ComputerFieldsSettingsViewModelBuilder>().InSingletonScope();
            this.Bind<IServerFieldsSettingsViewModelBuilder>().To<ServerFieldsSettingsViewModelBuilder>().InSingletonScope();
            this.Bind<IPrinterFieldsSettingsViewModelBuilder>().To<PrinterFieldsSettingsViewModelBuilder>().InSingletonScope();
            this.Bind<IInventoryFieldSettingsEditViewModelBuilder>().To<InventoryFieldSettingsEditViewModelBuilder>().InSingletonScope();

            this.Bind<IComputerBuilder>().To<ComputerBuilder>().InSingletonScope();
            this.Bind<IComputerCopyBuilder>().To<ComputerCopyBuilder>();

            this.Bind<IComputerRestorer>().To<ComputerRestorer>().InSingletonScope();
            this.Bind<IComputerValidator>().To<ComputerValidator>().InSingletonScope();
            this.Bind<IComputerCopyService>().To<ComputerCopyService>().InSingletonScope();

            this.Bind<IServerBuilder>().To<ServerBuilder>().InSingletonScope();

            this.Bind<IServerRestorer>().To<ServerRestorer>().InSingletonScope();
            this.Bind<IServerValidator>().To<ServerValidator>().InSingletonScope();

            this.Bind<IPrinterBuilder>().To<PrinterBuilder>().InSingletonScope();

            this.Bind<IPrinterRestorer>().To<PrinterRestorer>().InSingletonScope();
            this.Bind<IPrinterValidator>().To<PrinterValidator>().InSingletonScope();

            this.Bind<IInventoryModelBuilder>().To<InventoryModelBuilder>();
            this.Bind<IInventoryValueBuilder>().To<InventoryValueBuilder>();

            this.Bind<IInventoryRestorer>().To<InventoryRestorer>();
            this.Bind<IInventoryValidator>().To<InventoryValidator>();
        }
    }
}