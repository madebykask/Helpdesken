namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterViewModel
    {
        public PrinterViewModel(
            Printer printer,
            ConfigurableFieldModel<SelectList> departments,
            ConfigurableFieldModel<SelectList> buildings,
            ConfigurableFieldModel<SelectList> floors,
            ConfigurableFieldModel<SelectList> rooms)
        {
            this.Printer = printer;
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        [NotNull]
        public Printer Printer { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Departments { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Buildings { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Floors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Rooms { get; set; }
    }
}