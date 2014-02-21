namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(FieldSettingOverview userIdFieldSetting)
        {
            this.UserIdFieldSetting = userIdFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview UserIdFieldSetting { get; set; }
    }
}