namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class SoundFieldsSettingsModel
    {
        public SoundFieldsSettingsModel()
        {
        }

        public SoundFieldsSettingsModel(FieldSettingModel soundCardFieldSettingModel)
        {
            this.SoundCardFieldSettingModel = soundCardFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Sound Card")]
        public FieldSettingModel SoundCardFieldSettingModel { get; set; }
    }
}