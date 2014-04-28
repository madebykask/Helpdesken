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

        public List<InventoryTypeModel> InventoryTypes { get; set; }
    }
}