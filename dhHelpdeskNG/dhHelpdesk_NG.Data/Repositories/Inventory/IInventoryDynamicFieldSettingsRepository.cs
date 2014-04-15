namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;

    public interface IInventoryDynamicFieldSettingsRepository
    {
        void Add(InventoryDynamicFieldSetting businessModel);

        void DeleteById(int id);

        void Update(InventoryDynamicFieldSetting businessModel);

        List<InventoryDynamicFieldSetting> GetFieldSettingsForEdit(int inventoryTypeId);

        List<InventoryDynamicFieldSettingForModelEdit> GetFieldSettingsForModelEdit(int inventoryTypeId);

        List<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int inventoryTypeId);
    }
}