namespace DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public class ComputerEditOptions
    {
        public ComputerEditOptions(
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
            List<ItemOverview> rooms,
            List<ItemOverview> computerContractStatuses,
            List<ItemOverview> computerStatuses)
        {
            ComputerModels = computerModels;
            ComputerTypes = computerTypes;
            OperatingSystems = operatingSystems;
            Processors = processors;
            Rams = rams;
            NetworkAdapters = networkAdapters;
            Departments = departments;
            Domains = domains;
            Units = units;
            Buildings = buildings;
            Floors = floors;
            Rooms = rooms;
            ComputerContractStatuses = computerContractStatuses;
            ComputerStatuses = computerStatuses;
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
        public List<ItemOverview> ComputerContractStatuses { get; private set; }
        public List<ItemOverview> ComputerStatuses { get; private set; }
    }
}