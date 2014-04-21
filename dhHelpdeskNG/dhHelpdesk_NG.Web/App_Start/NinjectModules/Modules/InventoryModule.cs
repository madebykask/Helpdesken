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
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Computer;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Printer;
    using DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Server;

    using Ninject.Modules;

    public class InventoryModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit>>().To<ComputerFieldSettingsToComputerEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview>>().To<ComputerFieldSettingsToComputerOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings>>().To<ComputerFieldSettingsToFieldSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter>>().To<ComputerFieldSettingsToSearchSettingsMapper>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ServerFieldsSettingsForModelEdit>>().To<ServerFieldSettingsToServerEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ServerFieldsSettingsOverview>>().To<ServerFieldSettingsToServerOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ServerFieldsSettings>>().To<ServerFieldSettingsToFieldSettingsMapper>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, PrinterFieldsSettingsForModelEdit>>().To<PrinterFieldSettingsToPrinterEditSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, PrinterFieldsSettingsOverview>>().To<PrinterFieldSettingsToPrinterOverviewSettingsMapper>().InSingletonScope();
            this.Bind<IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, PrinterFieldsSettings>>().To<PrinterFieldSettingsToFieldSettingsMapper>().InSingletonScope();
        }
    }
}