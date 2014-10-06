namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel()
        {
        }

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
        [LocalizedDisplay("Created Date")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Changed Date")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Synchronize Change Date")]
        public FieldSettingModel SyncChangeDateFieldSettingModel { get; set; }
    }
}