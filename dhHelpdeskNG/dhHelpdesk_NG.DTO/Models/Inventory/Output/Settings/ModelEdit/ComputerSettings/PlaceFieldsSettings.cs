namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(ModelEditFieldSetting roomFieldSetting, ModelEditFieldSetting buildingFieldSetting, ModelEditFieldSetting floorFieldSetting,
            ModelEditFieldSetting addressFieldSetting, ModelEditFieldSetting postalCodeFieldSetting, 
            ModelEditFieldSetting postalAddressFieldSetting, ModelEditFieldSetting placeFieldSetting, 
            ModelEditFieldSetting place2FieldSetting)
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
        public ModelEditFieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting BuildingFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting FloorFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting AddressFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PostalCodeFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PostalAddressFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PlaceFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting Place2FieldSetting { get; set; }
    }
}