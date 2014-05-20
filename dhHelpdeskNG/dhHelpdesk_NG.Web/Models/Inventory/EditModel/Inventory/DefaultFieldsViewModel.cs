namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldsViewModel
    {
        public DefaultFieldsViewModel(
            DefaultFieldsModel defaultFieldsModel,
            ConfigurableFieldModel<SelectList> departments,
            SelectList buildings,
            SelectList floors,
            ConfigurableFieldModel<SelectList> rooms)
        {
            this.DefaultFieldsModel = defaultFieldsModel;
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        [NotNull]
        public DefaultFieldsModel DefaultFieldsModel { get; set; }

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