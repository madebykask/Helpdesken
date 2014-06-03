namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class DateFieldsSettingsModel
    {
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
        [LocalizedDisplay("Created Date")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Changed Date")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Synchronize Changed Date")]
        public FieldSettingModel SyncChangedDateSettingModel { get; set; }

        [LocalizedDisplay("Scan Date")]
        public FieldSettingModel ScanDateFieldSettingModel { get; set; }

        [LocalizedDisplay("Path Directory")]
        public FieldSettingModel PathDirectoryFieldSettingModel { get; set; }
    }
}