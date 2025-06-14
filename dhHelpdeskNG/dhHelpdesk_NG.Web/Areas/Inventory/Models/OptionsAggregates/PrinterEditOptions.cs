namespace DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public class PrinterEditOptions
    {
        public PrinterEditOptions(
            List<ItemOverview> departments,
            List<ItemOverview> buildings,
            List<ItemOverview> floors,
            List<ItemOverview> rooms)
        {
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        public List<ItemOverview> Departments { get; private set; }

        public List<ItemOverview> Buildings { get; private set; }

        public List<ItemOverview> Floors { get; private set; }

        public List<ItemOverview> Rooms { get; private set; }
    }
}