namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsViewModel
    {
        public PlaceFieldsViewModel(
            PlaceFieldsModel placeFieldsModel,
            SelectList buildings,
            SelectList floors,
            ConfigurableFieldModel<SelectList> rooms)
        {
            this.PlaceFieldsModel = placeFieldsModel;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        public PlaceFieldsModel PlaceFieldsModel { get; set; }

        [NotNull]
        public SelectList Buildings { get; set; }

        [NotNull]
        public SelectList Floors { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Rooms { get; set; }
    }
}