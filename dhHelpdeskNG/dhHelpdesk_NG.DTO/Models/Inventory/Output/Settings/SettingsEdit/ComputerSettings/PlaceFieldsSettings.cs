namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(FieldSetting roomFieldSetting, FieldSetting addressFieldSetting, FieldSetting postalCodeFieldSetting, FieldSetting postalAddressFieldSetting, FieldSetting placeFieldSetting, FieldSetting place2FieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.AddressFieldSetting = addressFieldSetting;
            this.PostalCodeFieldSetting = postalCodeFieldSetting;
            this.PostalAddressFieldSetting = postalAddressFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.Place2FieldSetting = place2FieldSetting;
        }

        [NotNull]
        public FieldSetting RoomFieldSetting { get; set; }

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