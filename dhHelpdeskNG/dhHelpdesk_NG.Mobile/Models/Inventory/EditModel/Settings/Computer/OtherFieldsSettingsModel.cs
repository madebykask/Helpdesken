namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class OtherFieldsSettingsModel
    {
        public OtherFieldsSettingsModel()
        {
        }

        public OtherFieldsSettingsModel(FieldSettingModel infoFieldSettingModel)
        {
            this.InfoFieldSettingModel = infoFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Info")]
        public FieldSettingModel InfoFieldSettingModel { get; set; }
    }
}