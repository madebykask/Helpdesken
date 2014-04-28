namespace DH.Helpdesk.Web.Models.Changes.InventoryDialog
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

        public InventoryTypeModel(string name, MultiSelectList inventories)
        {
            this.Name = name;
            this.Inventories = inventories;
        }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public List<int> SelectedInventoryIds { get; set; }

        public MultiSelectList Inventories { get; set; }
    }
}