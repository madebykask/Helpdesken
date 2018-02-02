namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryDynamicFieldSettingsRepository : INewRepository
    {
        void Add(InventoryDynamicFieldSetting businessModel);

        void DeleteById(int id);

        void Update(InventoryDynamicFieldSetting businessModel);

        void Update(List<InventoryDynamicFieldSetting> businessModels);

        List<InventoryDynamicFieldSetting> GetFieldSettingsForEdit(int inventoryTypeId);

        List<InventoryDynamicFieldSettingForModelEdit> GetFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false);

        List<InventoryDynamicFieldSettingOverviewWithType> GetFieldSettingsOverviewWithType(List<int> inventoryTypeIds);

        List<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int inventoryTypeId);

        void DeleteByInventoryTypeId(int inventoryTypeId);
    }
}