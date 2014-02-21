namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsSettings
    {
        public ProccesorFieldsSettings(FieldSetting proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public FieldSetting ProccesorFieldSetting { get; set; }
    }
}