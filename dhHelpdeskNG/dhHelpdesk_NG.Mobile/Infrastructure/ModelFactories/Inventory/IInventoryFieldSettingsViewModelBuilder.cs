namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Inventory;

    public interface IInventoryFieldSettingsEditViewModelBuilder
    {
        InventoryFieldSettingsEditViewModel BuildViewModel(
            InventoryType inventoryType,
            InventoryFieldSettingsForEditResponse response,
            List<TypeGroupModel> groupModels);

        InventoryFieldSettingsEditViewModel BuildDefaultViewModel(
            List<TypeGroupModel> groupModels);
    }
}