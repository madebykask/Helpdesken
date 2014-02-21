namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsSettings
    {
        public OperatingSystemFieldsSettings(FieldSetting operatingSystemFieldSetting, FieldSetting versionFieldSetting, FieldSetting servicePackSystemFieldSetting, FieldSetting registrationCodeSystemFieldSetting, FieldSetting productKeyFieldSetting)
        {
            this.OperatingSystemFieldSetting = operatingSystemFieldSetting;
            this.VersionFieldSetting = versionFieldSetting;
            this.ServicePackSystemFieldSetting = servicePackSystemFieldSetting;
            this.RegistrationCodeSystemFieldSetting = registrationCodeSystemFieldSetting;
            this.ProductKeyFieldSetting = productKeyFieldSetting;
        }

        [NotNull]
        public FieldSetting OperatingSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSetting VersionFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ServicePackSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSetting RegistrationCodeSystemFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ProductKeyFieldSetting { get; set; }
    }
}