namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
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