namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsSettings
    {
        public ContactFieldsSettings(ModelEditFieldSetting nameFieldSetting, ModelEditFieldSetting phoneFieldSetting, ModelEditFieldSetting emailFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.PhoneFieldSetting = phoneFieldSetting;
            this.EmailFieldSetting = emailFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PhoneFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting EmailFieldSetting { get; set; }
    }
}