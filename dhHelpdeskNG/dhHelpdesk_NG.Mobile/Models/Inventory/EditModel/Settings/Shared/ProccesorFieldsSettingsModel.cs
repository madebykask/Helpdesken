namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

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