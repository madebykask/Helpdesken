namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(ProcessingFieldSetting userIdFieldSetting)
        {
            this.UserIdFieldSetting = userIdFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting UserIdFieldSetting { get; set; }
    }
}