namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(ModelEditFieldSetting userIdFieldSetting)
        {
            this.UserIdFieldSetting = userIdFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting UserIdFieldSetting { get; set; }
    }
}