namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsSettingsModel
    {
        public OperatingSystemFieldsSettingsModel(
            FieldSettingModel operatingSystemFieldSettingModel,
            FieldSettingModel versionFieldSettingModel,
            FieldSettingModel servicePackSystemFieldSettingModel,
            FieldSettingModel registrationCodeSystemFieldSettingModel,
            FieldSettingModel productKeyFieldSettingModel)
        {
            this.OperatingSystemFieldSettingModel = operatingSystemFieldSettingModel;
            this.VersionFieldSettingModel = versionFieldSettingModel;
            this.ServicePackSystemFieldSettingModel = servicePackSystemFieldSettingModel;
            this.RegistrationCodeSystemFieldSettingModel = registrationCodeSystemFieldSettingModel;
            this.ProductKeyFieldSettingModel = productKeyFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel OperatingSystemFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel VersionFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ServicePackSystemFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel RegistrationCodeSystemFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ProductKeyFieldSettingModel { get; set; }
    }
}