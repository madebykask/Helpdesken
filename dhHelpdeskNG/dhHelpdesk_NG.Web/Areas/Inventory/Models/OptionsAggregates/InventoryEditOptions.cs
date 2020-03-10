namespace DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public class InventoryEditOptions
    {
        public InventoryEditOptions(
            List<ItemOverview> departments,
            List<ItemOverview> buildings,
            List<ItemOverview> floors,
            List<ItemOverview> rooms,
            List<ItemOverview> computerTypes)
        {
            this.Departments = departments;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
            this.ComputerTypes = computerTypes;
        }

        public List<ItemOverview> Departments { get; private set; }

        public List<ItemOverview> Buildings { get; private set; }

        public List<ItemOverview> Floors { get; private set; }

        public List<ItemOverview> Rooms { get; private set; }

        public List<ItemOverview> ComputerTypes { get; private set; }
    }
}