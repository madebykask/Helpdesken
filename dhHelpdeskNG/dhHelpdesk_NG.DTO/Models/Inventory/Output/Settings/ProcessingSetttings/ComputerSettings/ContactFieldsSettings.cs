namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsSettings
    {
        public ContactFieldsSettings(ProcessingFieldSetting nameFieldSetting, ProcessingFieldSetting phoneFieldSetting, ProcessingFieldSetting emailFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.PhoneFieldSetting = phoneFieldSetting;
            this.EmailFieldSetting = emailFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PhoneFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting EmailFieldSetting { get; set; }
    }
}