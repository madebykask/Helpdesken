namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsSettings
    {
        public SoundFieldsSettings(ModelEditFieldSetting soundCardFieldSetting)
        {
            this.SoundCardFieldSetting = soundCardFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting SoundCardFieldSetting { get; set; }
    }
}