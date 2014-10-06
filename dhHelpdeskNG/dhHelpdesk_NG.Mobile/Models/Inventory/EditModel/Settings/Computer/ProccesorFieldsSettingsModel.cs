namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
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
        [LocalizedDisplay("Proccesor")]
        public FieldSettingModel ProccesorFieldSettingModel { get; set; }
    }
}