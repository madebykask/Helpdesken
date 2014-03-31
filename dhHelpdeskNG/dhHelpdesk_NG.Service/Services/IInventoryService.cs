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

        ComputerFiltersRequest GetComputerFilters(int customerId);

        ServerFiltersRequest GetServerFilters(int customerId);

        PrinterFiltersRequest GetPrinterFilters(int customerId);

        CustomTypeFiltersRequest GetCustomTypeFilters(int customerId);

        void AddComputer(Computer businessModel);

        void DeleteComputer(int id);

        void UpdateComputer(Computer businessModel);

        Computer GetComputerById(int id);

        List<ComputerOverview> FindComputerOverviews(ComputersFilter computersFilter);

        void UpdateComputerFieldsSettings(ComputerFieldsSettings businessModel);

        ComputerFieldsSettings GetComputerFieldSettingsForEdit(int customerId, int languageId);

        ComputerFieldsSettingsForModelEdit GetComputerFieldSettingsForModelEdit(int customerId, int languageId);

        ComputerFieldsSettingsOverview GetComputerFieldSettingsOverview(int customerId, int languageId);
    }
}