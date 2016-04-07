namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class PlaceFieldsSettingsModel
    {
        public PlaceFieldsSettingsModel()
        {
        }

        public PlaceFieldsSettingsModel(FieldSettingModel roomFieldSettingModel, FieldSettingModel locationFieldSettingModel)
        {
            this.RoomFieldSettingModel = roomFieldSettingModel;
            this.LocationFieldSettingModel = locationFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Rum")]
        public FieldSettingModel RoomFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Placering")]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}