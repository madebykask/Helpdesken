namespace DH.Helpdesk.Mobile.Models.Changes.InventoryDialog
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InventoryTypeModel
    {
        public InventoryTypeModel()
        {
            this.SelectedInventoryIds = new List<int>();
        }

        public InventoryTypeModel(int id, string name, MultiSelectList inventories)
        {
            this.Id = id;
            this.Name = name;
            this.Inventories = inventories;
        }

        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public List<int> SelectedInventoryIds { get; set; }

        public MultiSelectList Inventories { get; set; }
    }
}