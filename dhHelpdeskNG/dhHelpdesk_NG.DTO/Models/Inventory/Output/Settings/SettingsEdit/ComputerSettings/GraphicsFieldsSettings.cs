namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsSettings
    {
        public GraphicsFieldsSettings(FieldSetting videoCardFieldSetting)
        {
            this.VideoCardFieldSetting = videoCardFieldSetting;
        }

        [NotNull]
        public FieldSetting VideoCardFieldSetting { get; set; }
    }
}