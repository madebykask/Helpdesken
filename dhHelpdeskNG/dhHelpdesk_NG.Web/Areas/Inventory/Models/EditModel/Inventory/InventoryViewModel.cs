namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryViewModel
    {
        public InventoryViewModel()
        {
        }

        public InventoryViewModel(int inventoryTypeId, DefaultFieldsViewModel defaultFieldsViewModel)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DefaultFieldsViewModel = defaultFieldsViewModel;
        }

        [IsId]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangeDate { get; set; }

        [IsId]
        public int InventoryTypeId { get; set; }

        [NotNull]
        public DefaultFieldsViewModel DefaultFieldsViewModel { get; set; }

        public bool IsForDialog { get; set; }
    }
}