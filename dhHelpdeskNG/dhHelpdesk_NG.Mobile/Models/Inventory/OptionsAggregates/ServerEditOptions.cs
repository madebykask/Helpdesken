namespace DH.Helpdesk.Web.Models.Inventory.OptionsAggregates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public class ServerEditOptions
    {
        public ServerEditOptions(
            List<ItemOverview> operatingSystems,
            List<ItemOverview> processors,
            List<ItemOverview> rams,
            List<ItemOverview> networkAdapters,
            List<ItemOverview> buildings,
            List<ItemOverview> floors,
            List<ItemOverview> rooms)
        {
            this.OperatingSystems = operatingSystems;
            this.Processors = processors;
            this.Rams = rams;
            this.NetworkAdapters = networkAdapters;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        public List<ItemOverview> OperatingSystems { get; private set; }

        public List<ItemOverview> Processors { get; private set; }

        public List<ItemOverview> Rams { get; private set; }

        public List<ItemOverview> NetworkAdapters { get; private set; }

        public List<ItemOverview> Buildings { get; private set; }

        public List<ItemOverview> Floors { get; private set; }

        public List<ItemOverview> Rooms { get; private set; }
    }
}