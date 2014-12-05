namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class PlaceFieldsSettingsModel
    {
        public PlaceFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Room")]
        public FieldSettingModel RoomFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Address")]
        public FieldSettingModel AddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Postal Code")]
        public FieldSettingModel PostalCodeFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Postal Address")]
        public FieldSettingModel PostalAddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Location")]

        public FieldSettingModel PlaceFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Location2")]
        public FieldSettingModel Place2FieldSettingModel { get; set; }
    }
}