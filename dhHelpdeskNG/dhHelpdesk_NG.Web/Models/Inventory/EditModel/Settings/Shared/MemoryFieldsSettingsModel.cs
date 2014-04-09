namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsSettingsModel
    {
        public MemoryFieldsSettingsModel(FieldSettingModel ramFieldSettingModel)
        {
            this.RAMFieldSettingModel = ramFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel RAMFieldSettingModel { get; set; }
    }
}