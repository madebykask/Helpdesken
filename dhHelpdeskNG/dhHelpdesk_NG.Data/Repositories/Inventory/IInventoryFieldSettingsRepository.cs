namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryFieldSettingsRepository : INewRepository
    {
        void Add(InventoryFieldSettings businessModel);

        void Update(InventoryFieldSettings businessModel);

        void DeleteByInventoryTypeId(int inventoryTypeId);

        InventoryFieldSettings GetFieldSettingsForEdit(int inventoryTypeId);

        InventoryFieldSettingsForModelEdit GetFieldSettingsForModelEdit(int inventoryTypeId, bool isReadonly = false);

        List<InventoryFieldSettingsOverviewWithType> GetFieldSettingsOverviews(List<int> inventoryTypeIds);

        InventoryFieldSettingsOverview GetFieldSettingsOverview(int inventoryTypeId);

        InventoryFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int inventoryTypeId);

        InventoryFieldSettingsProcessing GetFieldSettingsForProcessing(int inventoryTypeId);
    }
}