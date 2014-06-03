namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory;

    public interface IInventoryFieldSettingsViewModelBuilder
    {
        InventoryFieldSettingsViewModel BuildViewModel(
            int inventoryTypeId,
            InventoryFieldSettingsForEditResponse response,
            List<TypeGroupModel> groupModels);

        NewInventoryFieldSettingsViewModel BuildDefaultViewModel(
            int inventoryTypeId,
            List<TypeGroupModel> groupModels);
    }
}