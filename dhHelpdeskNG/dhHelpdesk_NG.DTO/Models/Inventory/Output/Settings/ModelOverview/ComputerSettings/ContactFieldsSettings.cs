namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsSettings
    {
        public ContactFieldsSettings(FieldSettingOverview nameFieldSetting, FieldSettingOverview phoneFieldSetting, FieldSettingOverview emailFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.PhoneFieldSetting = phoneFieldSetting;
            this.EmailFieldSetting = emailFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NameFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PhoneFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview EmailFieldSetting { get; set; }
    }
}