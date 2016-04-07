namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
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
        [LocalizedDisplay("Skapad datum")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Senast ändrad datum")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Synkroniserad datum")]
        public FieldSettingModel SyncChangeDateFieldSettingModel { get; set; }
    }
}