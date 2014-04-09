namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SyncChangedDateSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ScanDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PathDirectoryFieldSettingModel { get; set; }
    }
}