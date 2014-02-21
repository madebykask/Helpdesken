namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsSettings
    {
        public OperatingSystemFieldsSettings(ModelEditFieldSetting operatingSystemFieldSetting, ModelEditFieldSetting versionFieldSetting, ModelEditFieldSetting servicePackSystemFieldSetting, ModelEditFieldSetting registrationCodeSystemFieldSetting, ModelEditFieldSetting productKeyFieldSetting)
        {
            this.OperatingSystemFieldSetting = operatingSystemFieldSetting;
            this.VersionFieldSetting = versionFieldSetting;
            this.ServicePackSystemFieldSetting = servicePackSystemFieldSetting;
            this.RegistrationCodeSystemFieldSetting = registrationCodeSystemFieldSetting;
            this.ProductKeyFieldSetting = productKeyFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting OperatingSystemFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting VersionFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ServicePackSystemFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting RegistrationCodeSystemFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ProductKeyFieldSetting { get; set; }
    }
}