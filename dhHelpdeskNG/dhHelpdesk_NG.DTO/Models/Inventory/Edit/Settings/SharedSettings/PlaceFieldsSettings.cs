namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(FieldSetting roomFieldSetting, FieldSetting locationFieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public FieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public FieldSetting LocationFieldSetting { get; set; }
    }
}