namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ContactInformationFieldsSettingsModel
    {
        public ContactInformationFieldsSettingsModel(FieldSettingModel userIdFieldSettingModel)
        {
            this.UserIdFieldSettingModel = userIdFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("User Id")]
        public FieldSettingModel UserIdFieldSettingModel { get; set; }
    }
}