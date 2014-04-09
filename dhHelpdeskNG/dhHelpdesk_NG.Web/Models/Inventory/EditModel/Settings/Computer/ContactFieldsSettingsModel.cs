namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsSettingsModel
    {
        public ContactFieldsSettingsModel(
            FieldSettingModel nameFieldSettingModel,
            FieldSettingModel phoneFieldSettingModel,
            FieldSettingModel emailFieldSettingModel)
        {
            this.NameFieldSettingModel = nameFieldSettingModel;
            this.PhoneFieldSettingModel = phoneFieldSettingModel;
            this.EmailFieldSettingModel = emailFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PhoneFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel EmailFieldSettingModel { get; set; }
    }
}