namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(ProcessingFieldSetting roomFieldSetting, ProcessingFieldSetting buildingFieldSetting, ProcessingFieldSetting floorFieldSetting,
            ProcessingFieldSetting addressFieldSetting, ProcessingFieldSetting postalCodeFieldSetting, 
            ProcessingFieldSetting postalAddressFieldSetting, ProcessingFieldSetting placeFieldSetting, ProcessingFieldSetting place2FieldSetting)
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
        public ProcessingFieldSetting FloorFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting BuildingFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AddressFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PostalCodeFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PostalAddressFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PlaceFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting Place2FieldSetting { get; set; }
    }
}