namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel(FieldSettingModel createdDateFieldSettingModel, FieldSettingModel changedDateFieldSettingModel)
        {
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }
    }
}