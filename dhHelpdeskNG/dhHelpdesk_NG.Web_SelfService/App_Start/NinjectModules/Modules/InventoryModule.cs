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
using DH.Helpdesk.Services.Services.Concrete;
using Ninject.Modules;

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
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
            
            this.Bind<IComputerRestorer>().To<ComputerRestorer>().InSingletonScope();
            this.Bind<IComputerValidator>().To<ComputerValidator>().InSingletonScope();
            this.Bind<IComputerCopyService>().To<ComputerCopyService>().InSingletonScope();

            this.Bind<IServerRestorer>().To<ServerRestorer>().InSingletonScope();
            this.Bind<IServerValidator>().To<ServerValidator>().InSingletonScope();

            this.Bind<IPrinterRestorer>().To<PrinterRestorer>().InSingletonScope();
            this.Bind<IPrinterValidator>().To<PrinterValidator>().InSingletonScope();

            this.Bind<IInventoryRestorer>().To<InventoryRestorer>();
            this.Bind<IInventoryValidator>().To<InventoryValidator>();
        }
    }
}