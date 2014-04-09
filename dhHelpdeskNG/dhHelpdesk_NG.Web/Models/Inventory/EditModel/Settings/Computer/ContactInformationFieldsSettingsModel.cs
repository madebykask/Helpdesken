namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettingsModel
    {
        public ContactInformationFieldsSettingsModel(FieldSettingModel userIdFieldSettingModel)
        {
            this.UserIdFieldSettingModel = userIdFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel UserIdFieldSettingModel { get; set; }
    }
}