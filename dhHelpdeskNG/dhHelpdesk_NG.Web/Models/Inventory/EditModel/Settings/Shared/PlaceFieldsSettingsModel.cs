namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class PlaceFieldsSettingsModel
    {
        public PlaceFieldsSettingsModel(FieldSettingModel roomFieldSettingModel, FieldSettingModel locationFieldSettingModel)
        {
            this.RoomFieldSettingModel = roomFieldSettingModel;
            this.LocationFieldSettingModel = locationFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Room")]
        public FieldSettingModel RoomFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Location")]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}