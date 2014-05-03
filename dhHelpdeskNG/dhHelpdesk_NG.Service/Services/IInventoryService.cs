namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public interface IInventoryService
    {
        List<ItemOverview> GetInventoryTypes(int customerId);

        #region Workstation

        ComputerFiltersResponse GetWorkstationFilters(int customerId);

        void AddWorkstation(Computer businessModel);

        void DeleteWorkstation(int id);

        void UpdateWorkstation(Computer businessModel);

        ComputerEditResponse GetComputerEditResponse(int id, int customerId, int langaugeId);

        List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter);

        void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel);

        ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId);

        ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId);

        ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId);

        ComputerFieldsSettingsOverviewForFilter GetWorkstationFieldSettingsOverviewForFilter(int customerId, int languageId);

        #endregion

        #region Server

        void AddServer(Server businessModel);

        void DeleteServer(int id);

        void UpdateWorkstation(Server businessModel);

        Server GetServerById(int id);

        List<ServerOverview> GetServers(ServersFilter computersFilter);

        void UpdateServerFieldsSettings(ServerFieldsSettings businessModel);

        ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId);

        ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId);

        ServerFieldsSettingsOverview GetServerFieldSettingsOverview(int customerId, int languageId);

        #endregion

        #region Printer

        PrinterFiltersResponse GetPrinterFilters(int customerId);

        void AddPrinter(Printer businessModel);

        void DeletePrinter(int id);

        void UpdatePrinter(Printer businessModel);

        Printer GetPrinterById(int id);

        List<PrinterOverview> GetPrinters(PrintersFilter printersFilter);

        void UpdatePrinterFieldsSettings(PrinterFieldsSettings businessModel);

        PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId);

        PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId);

        PrinterFieldsSettingsOverview GetPrinterFieldSettingsOverview(int customerId, int languageId);

        PrinterFieldsSettingsOverviewForFilter GetPrinterFieldSettingsOverviewForFilter(int customerId, int languageId);

        #endregion

        #region DynamicType

        CustomTypeFiltersResponse GetInventoryFilters(int customerId);

        InventoryOverviewResponse GetInventories(InventoriesFilter filter);

        InventoryFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(int inventoryTypeId);

        InventoryFieldsSettingsOverviewForFilter GetInventoryFieldSettingsOverviewForFilter(int inventoryTypeId);

        #endregion
    }
}