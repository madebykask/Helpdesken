namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ContactFieldsSettingsModel
    {
        public ContactFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Name")]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel PhoneFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Email")]
        public FieldSettingModel EmailFieldSettingModel { get; set; }
    }
}