namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Services.Response.Inventory;

    public interface IInventorySettingsService
    {
        void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel);

        ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId);

        ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false);

        ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId);

        ComputerFieldsSettingsOverviewForFilter GetWorkstationFieldSettingsOverviewForFilter(int customerId, int languageId);

        ComputerFieldsSettingsOverviewForShortInfo GetWorkstationFieldSettingsForShortInfo(int customerId, int languageId);

        void UpdateServerFieldsSettings(ServerFieldsSettings businessModel);

        ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId);

        ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId);

        ServerFieldsSettingsOverview GetServerFieldSettingsOverview(int customerId, int languageId);

        void UpdatePrinterFieldsSettings(PrinterFieldsSettings businessModel);

        PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId);

        PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId);

        PrinterFieldsSettingsOverview GetPrinterFieldSettingsOverview(int customerId, int languageId);

        PrinterFieldsSettingsOverviewForFilter GetPrinterFieldSettingsOverviewForFilter(int customerId, int languageId);

        void AddDynamicFieldSetting(InventoryDynamicFieldSetting businessModel);

        void UpdateDynamicFieldsSettings(List<InventoryDynamicFieldSetting> businessModels);

        void DeleteDynamicFieldSetting(int id);

        void AddInventoryFieldsSettings(InventoryFieldSettings businessModel);

        void UpdateInventoryFieldsSettings(InventoryFieldSettings businessModel);

        InventoryFieldSettingsForEditResponse GetInventoryFieldSettingsForEdit(int inventoryTypeId);

        InventoryFieldSettingsForModelEditResponse GetInventoryFieldSettingsForModelEdit(int inventoryTypeId);

        InventoryFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(int inventoryTypeId);

        InventoryFieldsSettingsOverviewForFilter GetInventoryFieldSettingsOverviewForFilter(int inventoryTypeId);

        InventoriesFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(List<int> invetoryTypeIds);
    }
}