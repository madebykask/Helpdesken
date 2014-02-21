namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsSettings
    {
        public SoundFieldsSettings(FieldSettingOverview soundCardFieldSetting)
        {
            this.SoundCardFieldSetting = soundCardFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview SoundCardFieldSetting { get; set; }
    }
}