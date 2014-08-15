namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsSettings
    {
        public OperatingSystemFieldsSettings(ProcessingFieldSetting operatingSystemFieldSetting, ProcessingFieldSetting versionFieldSetting, ProcessingFieldSetting servicePackSystemFieldSetting, ProcessingFieldSetting registrationCodeSystemFieldSetting, ProcessingFieldSetting productKeyFieldSetting)
        {
            this.OperatingSystemFieldSetting = operatingSystemFieldSetting;
            this.VersionFieldSetting = versionFieldSetting;
            this.ServicePackSystemFieldSetting = servicePackSystemFieldSetting;
            this.RegistrationCodeSystemFieldSetting = registrationCodeSystemFieldSetting;
            this.ProductKeyFieldSetting = productKeyFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting OperatingSystemFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting VersionFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ServicePackSystemFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting RegistrationCodeSystemFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ProductKeyFieldSetting { get; set; }
    }
}