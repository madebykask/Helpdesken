namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettingsModel
    {
        public PlaceFieldsSettingsModel(FieldSettingModel roomFieldSettingModel, FieldSettingModel locationFieldSettingModel)
        {
            this.RoomFieldSettingModel = roomFieldSettingModel;
            this.LocationFieldSettingModel = locationFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel RoomFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}