namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(ModelEditFieldSetting roomFieldSetting, ModelEditFieldSetting locationFieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting LocationFieldSetting { get; set; }
    }
}