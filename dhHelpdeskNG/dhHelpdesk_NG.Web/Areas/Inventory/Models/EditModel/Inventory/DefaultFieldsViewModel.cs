namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldsViewModel
    {
        public DefaultFieldsViewModel()
        {
        }

        public DefaultFieldsViewModel(
            DefaultFieldsModel defaultFieldsModel,
            SelectList departments,
            SelectList buildings,
            SelectList floors,
            SelectList rooms,
            SelectList computerTypes)
        {
            this.DefaultFieldsModel = defaultFieldsModel;
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public DefaultFieldsModel DefaultFieldsModel { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }

        [NotNull]
        public SelectList Buildings { get; set; }

        [NotNull]
        public SelectList Floors { get; set; }

        [NotNull]
        public SelectList Rooms { get; set; }

        [NotNull]
        public SelectList ComputerTypes { get; set; }
    }
}