namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsSettings
    {
        public ContactFieldsSettings(FieldSetting nameFieldSetting, FieldSetting phoneFieldSetting, FieldSetting emailFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.PhoneFieldSetting = phoneFieldSetting;
            this.EmailFieldSetting = emailFieldSetting;
        }

        [NotNull]
        public FieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PhoneFieldSetting { get; set; }

        [NotNull]
        public FieldSetting EmailFieldSetting { get; set; }
    }
}