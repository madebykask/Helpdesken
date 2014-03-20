namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;

    public class InventoryService : IInventoryService
    {
        public void AddComputer(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteComputer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateComputer(Computer businessModel)
        {
            throw new NotImplementedException();
        }

        public Computer GetComputerById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ComputerOverview> FindComputerOverviews(
            int customerId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateTime? contractStartDateFrom,
            DateTime? contractStartDateTo,
            DateTime? contractEndDateFrom,
            DateTime? contractEndDateTo,
            DateTime? scanDateFrom,
            DateTime? scanDateTo,
            DateTime? scrapDateFrom,
            DateTime? scrapDateTo,
            string searchFor)
        {
            throw new NotImplementedException();
        }

        public void UpdateComputerFieldsSettings(ComputerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettings GetComputerFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsForModelEdit GetComputerFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsOverview GetComputerFieldSettingsOverview(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }
    }
}