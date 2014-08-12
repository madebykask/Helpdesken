namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsSettings
    {
        public SoundFieldsSettings(ProcessingFieldSetting soundCardFieldSetting)
        {
            this.SoundCardFieldSetting = soundCardFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting SoundCardFieldSetting { get; set; }
    }
}