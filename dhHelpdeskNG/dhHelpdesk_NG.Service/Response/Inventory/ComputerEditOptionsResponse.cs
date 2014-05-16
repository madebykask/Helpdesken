namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public class ComputerEditOptionsResponse
    {
        public ComputerEditOptionsResponse(
            List<ItemOverview> computerModels,
            List<ItemOverview> computerTypes,
            List<ItemOverview> operatingSystems,
            List<ItemOverview> processors,
            List<ItemOverview> rams,
            List<ItemOverview> networkAdapters,
            List<ItemOverview> departments,
            List<ItemOverview> domains,
            List<ItemOverview> units,
            List<ItemOverview> buildings,
            List<ItemOverview> floors,
            List<ItemOverview> rooms)
        {
            this.ComputerModels = computerModels;
            this.ComputerTypes = computerTypes;
            this.OperatingSystems = operatingSystems;
            this.Processors = processors;
            this.Rams = rams;
            this.NetworkAdapters = networkAdapters;
            this.Departments = departments;
            this.Domains = domains;
            this.Units = units;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        public List<ItemOverview> ComputerModels { get; private set; }

        public List<ItemOverview> ComputerTypes { get; private set; }

        public List<ItemOverview> OperatingSystems { get; private set; }

        public List<ItemOverview> Processors { get; private set; }

        public List<ItemOverview> Rams { get; private set; }

        public List<ItemOverview> NetworkAdapters { get; private set; }

        public List<ItemOverview> Departments { get; private set; }

        public List<ItemOverview> Domains { get; private set; }

        public List<ItemOverview> Units { get; private set; }

        public List<ItemOverview> Buildings { get; private set; }

        public List<ItemOverview> Floors { get; private set; }

        public List<ItemOverview> Rooms { get; private set; }
    }
}