namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(FieldSetting userIdFieldSetting)
        {
            this.UserIdFieldSetting = userIdFieldSetting;
        }

        [NotNull]
        public FieldSetting UserIdFieldSetting { get; set; }
    }
}