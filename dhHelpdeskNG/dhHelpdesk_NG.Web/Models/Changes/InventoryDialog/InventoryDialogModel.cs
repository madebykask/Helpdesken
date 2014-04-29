namespace DH.Helpdesk.Web.Models.Changes.InventoryDialog
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InventoryDialogModel
    {
        public InventoryDialogModel()
        {
            this.InventoryTypes = new List<InventoryTypeModel>();
        }

        public InventoryDialogModel(List<InventoryTypeModel> inventoryTypes)
        {
            this.InventoryTypes = inventoryTypes;
        }

        public InventoryDialogModel(List<InventoryTypeModel> inventoryTypes, List<string> selectedInventories)
        {
            this.InventoryTypes = inventoryTypes;
            this.SelectedInventories = selectedInventories;
        }

        [NotNull]
        public List<InventoryTypeModel> InventoryTypes { get; set; }

        [NotNull]
        public List<string> SelectedInventories { get; set; }
    }
}