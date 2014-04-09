namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettingsModel
    {
        public OtherFieldsSettingsModel(FieldSettingModel infoFieldSettingModel)
        {
            this.InfoFieldSettingModel = infoFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel InfoFieldSettingModel { get; set; }
    }
}