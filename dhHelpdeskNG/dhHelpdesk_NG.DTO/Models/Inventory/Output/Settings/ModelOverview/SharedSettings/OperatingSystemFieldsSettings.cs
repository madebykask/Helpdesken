namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsSettings
    {
        public OperatingSystemFieldsSettings(FieldSettingOverview operatingSystemFieldSetting, FieldSettingOverview versionFieldSetting, FieldSettingOverview servicePackSystemFieldSetting, FieldSettingOverview registrationCodeSystemFieldSetting, FieldSettingOverview productKeyFieldSetting)
        {
            this.OperatingSystemFieldSetting = operatingSystemFieldSetting;
            this.VersionFieldSetting = versionFieldSetting;
            this.ServicePackSystemFieldSetting = servicePackSystemFieldSetting;
            this.RegistrationCodeSystemFieldSetting = registrationCodeSystemFieldSetting;
            this.ProductKeyFieldSetting = productKeyFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview OperatingSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview VersionFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ServicePackSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview RegistrationCodeSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ProductKeyFieldSetting { get; set; }
    }
}