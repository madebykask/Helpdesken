namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsSettingsModel
    {
        public ProccesorFieldsSettingsModel(FieldSettingModel proccesorFieldSettingsModel)
        {
            this.ProccesorFieldSettingModel = proccesorFieldSettingsModel;
        }

        [NotNull]
        public FieldSettingModel ProccesorFieldSettingModel { get; set; }
    }
}