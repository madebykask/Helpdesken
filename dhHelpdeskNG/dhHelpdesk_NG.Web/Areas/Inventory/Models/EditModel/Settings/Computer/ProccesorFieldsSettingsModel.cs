namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ProccesorFieldsSettingsModel
    {
        public ProccesorFieldsSettingsModel()
        {
        }

        public ProccesorFieldsSettingsModel(FieldSettingModel proccesorFieldSettingsModel)
        {
            this.ProccesorFieldSettingModel = proccesorFieldSettingsModel;
        }

        [NotNull]
        [LocalizedDisplay("Processor")]
        public FieldSettingModel ProccesorFieldSettingModel { get; set; }
    }
}