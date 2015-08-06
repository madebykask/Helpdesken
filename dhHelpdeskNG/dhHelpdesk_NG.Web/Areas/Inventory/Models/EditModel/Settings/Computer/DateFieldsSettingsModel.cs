namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class DateFieldsSettingsModel
    {
        public DateFieldsSettingsModel()
        {
        }

        public DateFieldsSettingsModel(
            FieldSettingModel createdDateFieldSettingModel,
            FieldSettingModel changedDateFieldSettingModel,
            FieldSettingModel syncChangedDateSettingModel,
            FieldSettingModel scanDateFieldSettingModel,
            FieldSettingModel pathDirectoryFieldSettingModel)
        {
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
            this.SyncChangedDateSettingModel = syncChangedDateSettingModel;
            this.ScanDateFieldSettingModel = scanDateFieldSettingModel;
            this.PathDirectoryFieldSettingModel = pathDirectoryFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Skapad datum")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Senast ändrad datum")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Synkroniseringsdatum")]
        public FieldSettingModel SyncChangedDateSettingModel { get; set; }

        [LocalizedDisplay("Scanningsdatum")]
        public FieldSettingModel ScanDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Sökväg katalog")]
        public FieldSettingModel PathDirectoryFieldSettingModel { get; set; }
    }
}