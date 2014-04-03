namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerViewModel
    {
        public ServerViewModel(
            Server server,
            ConfigurableFieldModel<SelectList> buildings,
            ConfigurableFieldModel<SelectList> floors,
            ConfigurableFieldModel<SelectList> rooms,
            ConfigurableFieldModel<SelectList> networkAdapters,
            ConfigurableFieldModel<SelectList> rams,
            ConfigurableFieldModel<SelectList> proccessors,
            ConfigurableFieldModel<SelectList> operatingSystems)
        {
            this.Server = server;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
            this.NetworkAdapters = networkAdapters;
            this.RAMs = rams;
            this.Proccessors = proccessors;
            this.OperatingSystems = operatingSystems;
        }

        [NotNull]
        public Server Server { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Buildings { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Floors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Rooms { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> NetworkAdapters { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> RAMs { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Proccessors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> OperatingSystems { get; set; }
    }
}