namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsSettings
    {
        public SoundFieldsSettings(FieldSetting soundCardFieldSetting)
        {
            this.SoundCardFieldSetting = soundCardFieldSetting;
        }

        [NotNull]
        public FieldSetting SoundCardFieldSetting { get; set; }
    }
}