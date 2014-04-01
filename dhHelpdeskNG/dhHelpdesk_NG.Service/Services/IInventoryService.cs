namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;

    public interface IInventoryService
    {
        List<ItemOverview> GetInventoryTypes(int customerId);

        #region Workstation

        ComputerFiltersRequest GetWorkstationFilters(int customerId);

        void AddWorkstation(Computer businessModel);

        void DeleteWorkstation(int id);

        void UpdateWorkstation(Computer businessModel);

        Computer GetWorkstationById(int id);

        List<ComputerOverview> GetWorkstations(ComputersFilter computersFilter);

        void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel);

        ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId);

        ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId);

        ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId);

        #endregion

        ServerFiltersRequest GetServerFilters(int customerId);

        PrinterFiltersRequest GetPrinterFilters(int customerId);

        CustomTypeFiltersRequest GetCustomTypeFilters(int customerId);
    }
}