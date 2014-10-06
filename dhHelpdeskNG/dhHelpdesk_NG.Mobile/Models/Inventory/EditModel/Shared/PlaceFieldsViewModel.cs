namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsViewModel
    {
        public PlaceFieldsViewModel()
        {
        }

        public PlaceFieldsViewModel(PlaceFieldsModel placeFieldsModel, SelectList buildings, SelectList floors, SelectList rooms)
        {
            this.PlaceFieldsModel = placeFieldsModel;
            this.Buildings = buildings;
            this.Floors = floors;
            this.Rooms = rooms;
        }

        [NotNull]
        public PlaceFieldsModel PlaceFieldsModel { get; set; }

        [NotNull]
        public SelectList Buildings { get; set; }

        [NotNull]
        public SelectList Floors { get; set; }

        [NotNull]
        public SelectList Rooms { get; set; }
    }
}