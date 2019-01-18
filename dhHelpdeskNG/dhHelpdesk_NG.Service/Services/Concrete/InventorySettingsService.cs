using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Enums.Inventory.Computer;

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
        private readonly IComputerFieldSettingsRepository _computerFieldSettingsRepository;
        private readonly IComputerTabsSettingsRepository _computerTabsSettingsRepository;
        private readonly IServerFieldSettingsRepository _serverFieldSettingsRepository;
        private readonly IPrinterFieldSettingsRepository _printerFieldSettingsRepository;
        private readonly IInventoryFieldSettingsRepository _fieldinventoryFieldSettingsRepository;
        private readonly IInventoryDynamicFieldSettingsRepository _inventoryDynamicFieldSettingsRepository;
        private readonly IInventoryTypePropertyValueRepository _inventoryTypePropertyValueRepository;

        public InventorySettingsService(
            IComputerFieldSettingsRepository computerFieldSettingsRepository,
            IComputerTabsSettingsRepository computerTabsSettingsRepository,
            IServerFieldSettingsRepository serverFieldSettingsRepository,
            IPrinterFieldSettingsRepository printerFieldSettingsRepository,
            IInventoryFieldSettingsRepository inventoryFieldSettingsRepository,
            IInventoryDynamicFieldSettingsRepository inventoryDynamicFieldSettingsRepository,
            IInventoryTypePropertyValueRepository inventoryTypePropertyValueRepository)
        {
            this._computerFieldSettingsRepository = computerFieldSettingsRepository;
            this._computerTabsSettingsRepository = computerTabsSettingsRepository;
            this._serverFieldSettingsRepository = serverFieldSettingsRepository;
            this._printerFieldSettingsRepository = printerFieldSettingsRepository;
            this._fieldinventoryFieldSettingsRepository = inventoryFieldSettingsRepository;
            this._inventoryDynamicFieldSettingsRepository = inventoryDynamicFieldSettingsRepository;
            this._inventoryTypePropertyValueRepository = inventoryTypePropertyValueRepository;
        }

        #region WorkstationSettings

        public void UpdateWorkstationFieldsSettings(ComputerFieldsSettings businessModel, WorkstationTabsSettings tabsSettings)
        {
            _computerFieldSettingsRepository.Update(businessModel);
            _computerTabsSettingsRepository.Update(tabsSettings.ComputersTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.StorageTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.SoftwareTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.HotFixesTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.AccessoriesTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.ComputerLogsTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerTabsSettingsRepository.Update(tabsSettings.RelatedCasesTabSetting, businessModel.CustomerId, businessModel.LanguageId);
            _computerFieldSettingsRepository.Commit();
        }

        public ComputerFieldsSettings GetWorkstationFieldSettingsForEdit(int customerId, int languageId)
        {
            return _computerFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public WorkstationTabsSettings GetWorkstationTabsSettingsForEdit(int customerId, int languageId)
        {
            var tabs = _computerTabsSettingsRepository.GetTabsSettingsForEdit(customerId, languageId);

            var computerTab = tabs.First(t => t.TabField == WorkstationTabs.Workstations);
            var storagesTab = tabs.First(t => t.TabField == WorkstationTabs.Storages);
            var softwaresTab = tabs.First(t => t.TabField == WorkstationTabs.Softwares);
            var hotfixesTab = tabs.First(t => t.TabField == WorkstationTabs.HotFixes);
            var accessoriesTab = tabs.First(t => t.TabField == WorkstationTabs.Accessories);
            var computerLogsTab = tabs.First(t => t.TabField == WorkstationTabs.ComputerLogs);
            var relatedCasesTab = tabs.First(t => t.TabField == WorkstationTabs.RelatedCases);

            return new WorkstationTabsSettings(ModelStates.ForEdit,
                new TabSetting(computerTab.TabField, computerTab.Show, computerTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? computerTab.TabField),
                new TabSetting(storagesTab.TabField, storagesTab.Show, storagesTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? storagesTab.TabField),
                new TabSetting(softwaresTab.TabField, softwaresTab.Show, softwaresTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? softwaresTab.TabField),
                new TabSetting(hotfixesTab.TabField, hotfixesTab.Show, hotfixesTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? hotfixesTab.TabField),
                new TabSetting(computerLogsTab.TabField, computerLogsTab.Show, computerLogsTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? computerLogsTab.TabField),
                new TabSetting(accessoriesTab.TabField, accessoriesTab.Show, accessoriesTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? accessoriesTab.TabField),
                new TabSetting(relatedCasesTab.TabField, relatedCasesTab.Show, relatedCasesTab.WorkstationTabSettingLanguages?.FirstOrDefault()?.Label ?? relatedCasesTab.TabField));
        }

        public ComputerFieldsSettingsForModelEdit GetWorkstationFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            var models = _computerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);
            
            return models;
        }

        public ComputerFieldsSettingsOverview GetWorkstationFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this._computerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverviewForFilter GetWorkstationFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this._computerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);

            return models;
        }

        public ComputerFieldsSettingsOverviewForShortInfo GetWorkstationFieldSettingsForShortInfo(int customerId, int languageId)
        {
            var models = this._computerFieldSettingsRepository.GetFieldSettingsOverviewForShortInfo(customerId, languageId);

            return models;
        }

        #endregion

        #region ServerSettings

        public void UpdateServerFieldsSettings(ServerFieldsSettings businessModel)
        {
            this._serverFieldSettingsRepository.Update(businessModel);
            this._serverFieldSettingsRepository.Commit();
        }

        public ServerFieldsSettings GetServerFieldSettingsForEdit(int customerId, int languageId)
        {
            return this._serverFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public ServerFieldsSettingsForModelEdit GetServerFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            var models = this._serverFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);

            return models;
        }

        public ServerFieldsSettingsOverview GetServerFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this._serverFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);

            return models;
        }

        #endregion

        #region PrinterSettings

        public void UpdatePrinterFieldsSettings(PrinterFieldsSettings businessModel)
        {
            this._printerFieldSettingsRepository.Update(businessModel);
            this._printerFieldSettingsRepository.Commit();
        }

        public PrinterFieldsSettings GetPrinterFieldSettingsForEdit(int customerId, int languageId)
        {
            return this._printerFieldSettingsRepository.GetFieldSettingsForEdit(customerId, languageId);
        }

        public PrinterFieldsSettingsForModelEdit GetPrinterFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            return this._printerFieldSettingsRepository.GetFieldSettingsForModelEdit(customerId, languageId, isReadonly);
        }

        public PrinterFieldsSettingsOverview GetPrinterFieldSettingsOverview(int customerId, int languageId)
        {
            var models = this._printerFieldSettingsRepository.GetFieldSettingsOverview(customerId, languageId);
            return models;
        }

        public PrinterFieldsSettingsOverviewForFilter GetPrinterFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var models = this._printerFieldSettingsRepository.GetFieldSettingsOverviewForFilter(customerId, languageId);
            return models;
        }

        #endregion

        #region DynamicInventorySettings

        public void AddDynamicFieldSetting(InventoryDynamicFieldSetting businessModel)
        {
            this._inventoryDynamicFieldSettingsRepository.Add(businessModel);
            this._inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void UpdateDynamicFieldsSettings(List<InventoryDynamicFieldSetting> businessModels)
        {
            this._inventoryDynamicFieldSettingsRepository.Update(businessModels);
            this._inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void DeleteDynamicFieldSetting(int id)
        {
            this._inventoryTypePropertyValueRepository.DeleteByInventoryTypePropertyId(id);
            this._inventoryTypePropertyValueRepository.Commit();

            this._inventoryDynamicFieldSettingsRepository.DeleteById(id);
            this._inventoryDynamicFieldSettingsRepository.Commit();
        }

        public void AddInventoryFieldsSettings(InventoryFieldSettings businessModel)
        {
            this._fieldinventoryFieldSettingsRepository.Add(businessModel);
            this._fieldinventoryFieldSettingsRepository.Commit();
        }

        public void UpdateInventoryFieldsSettings(InventoryFieldSettings businessModel)
        {
            this._fieldinventoryFieldSettingsRepository.Update(businessModel);
            this._fieldinventoryFieldSettingsRepository.Commit();
        }

        public InventoryFieldSettingsForEditResponse GetInventoryFieldSettingsForEdit(int inventoryTypeId)
        {
            var setings = this._fieldinventoryFieldSettingsRepository.GetFieldSettingsForEdit(inventoryTypeId);
            var dynamicSettings = this._inventoryDynamicFieldSettingsRepository.GetFieldSettingsForEdit(inventoryTypeId);

            var response = new InventoryFieldSettingsForEditResponse(setings, dynamicSettings);
            return response;
        }

        public InventoryFieldSettingsForModelEditResponse GetInventoryFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false)
        {
            var setings = this._fieldinventoryFieldSettingsRepository.GetFieldSettingsForModelEdit(inventoryTypeId, isReadonly);
            var dynamicSettings = this._inventoryDynamicFieldSettingsRepository.GetFieldSettingsForModelEdit(inventoryTypeId, isReadonly);

            var response = new InventoryFieldSettingsForModelEditResponse(setings, dynamicSettings);
            return response;
        }

        public InventoryFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(int inventoryTypeId)
        {
            var setings = this._fieldinventoryFieldSettingsRepository.GetFieldSettingsOverview(inventoryTypeId);
            var dynamicSettings = this._inventoryDynamicFieldSettingsRepository.GetFieldSettingsOverview(
                inventoryTypeId);

            var response = new InventoryFieldSettingsOverviewResponse(setings, dynamicSettings);

            return response;
        }

        public InventoryFieldsSettingsOverviewForFilter GetInventoryFieldSettingsOverviewForFilter(int inventoryTypeId)
        {
            var models = this._fieldinventoryFieldSettingsRepository.GetFieldSettingsOverviewForFilter(inventoryTypeId);

            return models;
        }

        public InventoriesFieldSettingsOverviewResponse GetInventoryFieldSettingsOverview(List<int> invetoryTypeIds)
        {
            var inventorySettings = this._fieldinventoryFieldSettingsRepository.GetFieldSettingsOverviews(invetoryTypeIds);
            var inventoryDynamicSettings = this._inventoryDynamicFieldSettingsRepository.GetFieldSettingsOverviewWithType(invetoryTypeIds);
            var inventorySettingsResponse = new InventoriesFieldSettingsOverviewResponse(
                inventorySettings,
                inventoryDynamicSettings);

            return inventorySettingsResponse;
        }

        #endregion
    }
}
