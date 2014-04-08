namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryViewModel
    {
        public InventoryViewModel(
            InventoryModel inventory,
            List<DynamicFieldModel> dynamicFieldModels,
            ConfigurableFieldModel<SelectList> departments,
            SelectList buildings,
            SelectList floors,
            ConfigurableFieldModel<SelectList> rooms)
        {
            this.Inventory = inventory;
            this.DynamicFieldModels = dynamicFieldModels;
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        [NotNull]
        public InventoryModel Inventory { get; set; }

        [NotNull]
        public List<DynamicFieldModel> DynamicFieldModels { get; set; }

        [NotNull]
        public List<TypeGroupModel> TypeGroupModels { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Departments { get; set; }

        [NotNull]
        public SelectList Buildings { get; set; }

        [NotNull]
        public SelectList Floors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Rooms { get; set; }
    }
}