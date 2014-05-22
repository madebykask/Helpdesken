namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Services.Response.Inventory;

    public class InventorySettingsService : IInventorySettingsService
    {
        private readonly IComputerFieldSettingsRepository computerFieldSettingsRepository;

        private readonly IServerFieldSettingsRepository serverFieldSettingsRepository;

        private readonly IPrinterFieldSettingsRepository printerFieldSettingsRepository;

        private readonly IInventoryFieldSettingsRepository inventoryFieldSettingsRepository;

        private readonly IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository;

        public InventorySettingsService(
            IComputerFieldSettingsRepository computerFieldSettingsRepository,
            IServerFieldSettingsRepository serverFieldSettingsRepository,
            IPrinterFieldSettingsRepository printerFieldSettingsRepository,
            IInventoryFieldSettingsRepository inventoryFieldSettingsRepository,
            IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository)
        {
            this.computerFieldSettingsRepository = computerFieldSettingsRepository;
            this.serverFieldSettingsRepository = serverFieldSettingsRepository;
            this.printerFieldSettingsRepository = printerFieldSettingsRepository;
            this.inventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            this.inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
        }

        #region WorkstationSettings

        public void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverviewForFilter GetWorkstationFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);

            return models;
        }

        #endregion

        #region ServerSettings

        public void UpdateServerFieldsSettings(ServerFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId)
        {
            var models = this.serverFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId);

            return models;
        }

        public ServerFieldsSettingsOverview GetServerFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.serverFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        #endregion

        #region PrinterSettings

        public void UpdatePrinterFieldsSettings(PrinterFieldsSettings businessModel)
        {
            throw new NotImplementedException();
        }

        public PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new NotImplementedException();
        }

        public PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId)
        {
            return this.printerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId);
        }

        public PrinterFieldsSettingsOverview GetPrinterFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this.printerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        public PrinterFieldsSettingsOverviewForFilter GetPrinterFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this.printerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);

            return models;
        }

        #endregion

        #region DynamicInventorySettings

        public InventoryFieldSettingsForModelEditResponse GetInventoryFieldSettingsForModelEdit(int inventoryTypeId)
        {
            var setings = this.inventoryFieldSettingsRepository.GetFieldSettingsForModelEdit(inventoryTypeId);
            var dynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsForModelEdit(
                inventoryTypeId);

            var response = new InventoryFieldSettingsForModelEditResponse(setings, dynamicSettings);

            return response;
        }

        public InventoryFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(int inventoryTypeId)
        {
            var setings = this.inventoryFieldSettingsRepository.GetFieldSettingsOverview(inventoryTypeId);
            var dynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsOverview(
                inventoryTypeId);

            var response = new InventoryFieldSettingsOverviewResponse(setings, dynamicSettings);

            return response;
        }

        public InventoryFieldsSettingsOverviewForFilter GetInventoryFieldSettingsOverviewForFilter(int inventoryTypeId)
        {
            var models = this.inventoryFieldSettingsRepository.GetFieldSettingsOverviewForFilter(inventoryTypeId);

            return models;
        }

        #endregion
    }
}
