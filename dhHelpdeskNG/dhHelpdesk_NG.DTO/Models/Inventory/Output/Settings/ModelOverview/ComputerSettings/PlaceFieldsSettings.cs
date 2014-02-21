namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(FieldSettingOverview roomFieldSetting, FieldSettingOverview addressFieldSetting, FieldSettingOverview postalCodeFieldSetting, FieldSettingOverview postalAddressFieldSetting, FieldSettingOverview placeFieldSetting, FieldSettingOverview place2FieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.AddressFieldSetting = addressFieldSetting;
            this.PostalCodeFieldSetting = postalCodeFieldSetting;
            this.PostalAddressFieldSetting = postalAddressFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.Place2FieldSetting = place2FieldSetting;
        }

        [NotNull]
        public FieldSettingOverview RoomFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AddressFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PostalCodeFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PostalAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PlaceFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview Place2FieldSetting { get; set; }
    }
}