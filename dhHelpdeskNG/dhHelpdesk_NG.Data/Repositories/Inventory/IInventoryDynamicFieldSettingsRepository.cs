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

        InventoryDynamicFieldSetting GetFieldSettingsForEdit(int customerId, int inventoryTypeId);

        InventoryDynamicFieldSettingForModelEdit GetFieldSettingsForModelEdit(int customerId, int inventoryTypeId);

        IList<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int customerId, int inventoryTypeId);
    }
}