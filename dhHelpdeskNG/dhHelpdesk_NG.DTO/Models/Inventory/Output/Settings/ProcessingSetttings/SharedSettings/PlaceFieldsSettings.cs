namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(ProcessingFieldSetting roomFieldSetting, ProcessingFieldSetting locationFieldSetting)
        {
            this.RoomFieldSetting = roomFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting RoomFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting LocationFieldSetting { get; set; }
    }
}