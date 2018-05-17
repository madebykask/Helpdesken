namespace DH.Helpdesk.Services.Services.Concrete
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

        private readonly IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository;

        public InventorySettingsService(
            IComputerFieldSettingsRepository computerFieldSettingsRepository,
            IServerFieldSettingsRepository serverFieldSettingsRepository,
            IPrinterFieldSettingsRepository printerFieldSettingsRepository,
            IInventoryFieldSettingsRepository inventoryFieldSettingsRepository,
            IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository,
            IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository)
        {
            this.computerFieldSettingsRepository = computerFieldSettingsRepository;
            this.serverFieldSettingsRepository = serverFieldSettingsRepository;
            this.printerFieldSettingsRepository = printerFieldSettingsRepository;
            this.inventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            this.inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
            this.inventoryTypePropertyValueRepository = inventoryTypePropertyValueRepository;
        }

        #region WorkstationSettings

        public void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel)
        {
            this.computerFieldSettingsRepository.Update(businessModel);
            this.computerFieldSettingsRepository.Commit();
        }

        public ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId)
        {
            return this.computerFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);

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

        public ComputerFieldsSettingsOverviewForShortInfo GetWorkstationFieldSettingsForShortInfo(int customerId, int languageId)
        {
            var models = this.computerFieldSettingsRepository.GetFieldSettingsOverviewForShortInfo(customerId, languageId);

            return models;
        }

        #endregion

        #region ServerSettings

        public void UpdateServerFieldsSettings(ServerFieldsSettings businessModel)
        {
            this.serverFieldSettingsRepository.Update(businessModel);
            this.serverFieldSettingsRepository.Commit();
        }

        public ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId)
        {
            return this.serverFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            var models = this.serverFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);

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
            this.printerFieldSettingsRepository.Update(businessModel);
            this.printerFieldSettingsRepository.Commit();
        }

        public PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId)
        {
            return this.printerFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            return this.printerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);
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

        public void AddDynamicFieldSetting(InventoryDynamicFieldSetting businessModel)
        {
            this.inventoryDynamicFieldSettingsRepository.Add(businessModel);
            this.inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void UpdateDynamicFieldsSettings(List<InventoryDynamicFieldSetting> businessModels)
        {
            this.inventoryDynamicFieldSettingsRepository.Update(businessModels);
            this.inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void DeleteDynamicFieldSetting(int id)
        {
            this.inventoryTypePropertyValueRepository.DeleteByInventoryTypePropertyId(id);
            this.inventoryTypePropertyValueRepository.Commit();

            this.inventoryDynamicFieldSettingsRepository.DeleteById(id);
            this.inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void AddInventoryFieldsSettings(InventoryFieldSettings businessModel)
        {
            this.inventoryFieldSettingsRepository.Add(businessModel);
            this.inventoryFieldSettingsRepository.Commit();
        }

        public void UpdateInventoryFieldsSettings(InventoryFieldSettings businessModel)
        {
            this.inventoryFieldSettingsRepository.Update(businessModel);
            this.inventoryFieldSettingsRepository.Commit();
        }

        public InventoryFieldSettingsForEditResponse GetInventoryFieldSettingsForEdit(int inventoryTypeId)
        {
            var setings = this.inventoryFieldSettingsRepository.GetFieldSettingsForEdit(inventoryTypeId);
            var dynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsForEdit(inventoryTypeId);

            var response = new InventoryFieldSettingsForEditResponse(setings, dynamicSettings);
            return response;
        }

        public InventoryFieldSettingsForModelEditResponse GetInventoryFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false)
        {
            var setings = this.inventoryFieldSettingsRepository.GetFieldSettingsForModelEdit(inventoryTypeId, isReadonly);
            var dynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsForModelEdit(inventoryTypeId, isReadonly);

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

        public InventoriesFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(List<int> invetoryTypeIds)
        {
            var inventorySettings = this.inventoryFieldSettingsRepository.GetFieldSettingsOverviews(invetoryTypeIds);
            var inventoryDynamicSettings = this.inventoryDynamicFieldSettingsRepository.GetFieldSettingsOverviewWithType(invetoryTypeIds);
            var inventorySettingsResponse = new InventoriesFieldSettingsOverviewResponse(
                inventorySettings,
                inventoryDynamicSettings);

            return inventorySettingsResponse;
        }

        #endregion
    }
}
