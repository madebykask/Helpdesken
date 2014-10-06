namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class OperatingSystemFieldsSettingsModel
    {
        public OperatingSystemFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Operating System")]
        public FieldSettingModel OperatingSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Version")]
        public FieldSettingModel VersionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Service Pack")]
        public FieldSettingModel ServicePackSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Registration Code")]
        public FieldSettingModel RegistrationCodeSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Product Key")]
        public FieldSettingModel ProductKeyFieldSettingModel { get; set; }
    }
}