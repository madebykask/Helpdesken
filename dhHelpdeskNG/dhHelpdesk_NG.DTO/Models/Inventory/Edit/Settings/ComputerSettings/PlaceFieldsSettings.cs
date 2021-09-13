namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(FieldSetting roomFieldSetting, FieldSetting buildingFieldSetting, FieldSetting floorFieldSetting, FieldSetting addressFieldSetting, 
            FieldSetting postalCodeFieldSetting, FieldSetting postalAddressFieldSetting, 
            FieldSetting placeFieldSetting, FieldSetting place2FieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.BuildingFieldSetting = buildingFieldSetting;
            this.FloorFieldSetting = floorFieldSetting;
            this.AddressFieldSetting = addressFieldSetting;
            this.PostalCodeFieldSetting = postalCodeFieldSetting;
            this.PostalAddressFieldSetting = postalAddressFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.Place2FieldSetting = place2FieldSetting;
        }

        [NotNull]
        public FieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public FieldSetting BuildingFieldSetting { get; set; }

        [NotNull]
        public FieldSetting FloorFieldSetting { get; set; }

        [NotNull]
        public FieldSetting AddressFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PostalCodeFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PostalAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PlaceFieldSetting { get; set; }

        [NotNull]
        public FieldSetting Place2FieldSetting { get; set; }
    }
}