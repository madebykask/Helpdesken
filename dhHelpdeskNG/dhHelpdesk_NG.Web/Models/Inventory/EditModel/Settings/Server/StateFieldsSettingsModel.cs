namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel(
            FieldSettingModel createdDateFieldSettingModel,
            FieldSettingModel changedDateFieldSettingModel,
            FieldSettingModel syncChangeDateFieldSettingModel)
        {
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
            this.SyncChangeDateFieldSettingModel = syncChangeDateFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SyncChangeDateFieldSettingModel { get; set; }
    }
}