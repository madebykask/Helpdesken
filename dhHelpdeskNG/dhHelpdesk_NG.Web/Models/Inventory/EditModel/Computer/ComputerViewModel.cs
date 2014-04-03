namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerViewModel
    {
        public ComputerViewModel(
            Computer computer,
            ConfigurableFieldModel<SelectList> computerModels,
            ConfigurableFieldModel<SelectList> computerTypes,
            ConfigurableFieldModel<SelectList> states,
            ConfigurableFieldModel<SelectList> buildings,
            ConfigurableFieldModel<SelectList> floors,
            ConfigurableFieldModel<SelectList> rooms,
            ConfigurableFieldModel<SelectList> contractStatuses,
            ConfigurableFieldModel<SelectList> networkAdapters,
            ConfigurableFieldModel<SelectList> domains,
            ConfigurableFieldModel<SelectList> units,
            ConfigurableFieldModel<SelectList> rams,
            ConfigurableFieldModel<SelectList> proccessors,
            ConfigurableFieldModel<SelectList> operatingSystems)
        {
            this.Computer = computer;
            this.ComputerModels = computerModels;
            this.ComputerTypes = computerTypes;
            this.States = states;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
            this.ContractStatuses = contractStatuses;
            this.NetworkAdapters = networkAdapters;
            this.Domains = domains;
            this.Units = units;
            this.RAMs = rams;
            this.Proccessors = proccessors;
            this.OperatingSystems = operatingSystems;
        }

        [NotNull]
        public Computer Computer { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ComputerModels { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ComputerTypes { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> States { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Buildings { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Floors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Rooms { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ContractStatuses { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> NetworkAdapters { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Domains { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Units { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> RAMs { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Proccessors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> OperatingSystems { get; set; }
    }
}