namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class MemoryFieldsSettingsModel
    {
        public MemoryFieldsSettingsModel()
        {
        }

        public MemoryFieldsSettingsModel(FieldSettingModel ramFieldSettingModel)
        {
            this.RAMFieldSettingModel = ramFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("RAM")]
        public FieldSettingModel RAMFieldSettingModel { get; set; }
    }
}