namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(FieldSettingOverview roomFieldSetting, FieldSettingOverview locationFieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview RoomFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview LocationFieldSetting { get; set; }
    }
}