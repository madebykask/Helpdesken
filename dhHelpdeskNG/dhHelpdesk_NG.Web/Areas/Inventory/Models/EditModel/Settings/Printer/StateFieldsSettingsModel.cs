namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel()
        {
        }

        public StateFieldsSettingsModel(FieldSettingModel createdDateFieldSettingModel, 
                                        FieldSettingModel changedDateFieldSettingModel,
                                        FieldSettingModel syncdDateFieldSettingModel)
        {
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
            this.SyncDateFieldSettingModel = syncdDateFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Skapad datum")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Senast ändrad datum")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Synkroniseringsdatum")]
        public FieldSettingModel SyncDateFieldSettingModel { get; set; }
    }
}