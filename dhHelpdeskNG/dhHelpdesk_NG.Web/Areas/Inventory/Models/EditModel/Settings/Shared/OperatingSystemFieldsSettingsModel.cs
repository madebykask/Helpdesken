namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
        [LocalizedDisplay("Operativsystem")]
        public FieldSettingModel OperatingSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Version")]
        public FieldSettingModel VersionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Servicepack")]
        public FieldSettingModel ServicePackSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Registreringskod")]
        public FieldSettingModel RegistrationCodeSystemFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Produktnyckel")]
        public FieldSettingModel ProductKeyFieldSettingModel { get; set; }
    }
}