namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettingsModel
    {
        public PlaceFieldsSettingsModel(
            FieldSettingModel roomFieldSettingModel,
            FieldSettingModel addressFieldSettingModel,
            FieldSettingModel postalCodeFieldSettingModel,
            FieldSettingModel postalAddressFieldSettingModel,
            FieldSettingModel placeFieldSettingModel,
            FieldSettingModel place2FieldSettingModel)
        {
            this.RoomFieldSettingModel = roomFieldSettingModel;
            this.AddressFieldSettingModel = addressFieldSettingModel;
            this.PostalCodeFieldSettingModel = postalCodeFieldSettingModel;
            this.PostalAddressFieldSettingModel = postalAddressFieldSettingModel;
            this.PlaceFieldSettingModel = placeFieldSettingModel;
            this.Place2FieldSettingModel = place2FieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel RoomFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AddressFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PostalCodeFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PostalAddressFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PlaceFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel Place2FieldSettingModel { get; set; }
    }
}