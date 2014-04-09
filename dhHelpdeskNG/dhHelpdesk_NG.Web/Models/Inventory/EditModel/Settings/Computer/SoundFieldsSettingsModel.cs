namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsSettingsModel
    {
        public SoundFieldsSettingsModel(FieldSettingModel soundCardFieldSettingModel)
        {
            this.SoundCardFieldSettingModel = soundCardFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel SoundCardFieldSettingModel { get; set; }
    }
}