namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DateFieldsSettings
    {
        public DateFieldsSettings(FieldSettingOverview createdDateFieldSetting, FieldSettingOverview changedDateFieldSetting, FieldSettingOverview syncChangedDateSetting, FieldSettingOverview scanDateFieldSetting, FieldSettingOverview pathDirectoryFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangedDateSetting = syncChangedDateSetting;
            this.ScanDateFieldSetting = scanDateFieldSetting;
            this.PathDirectoryFieldSetting = pathDirectoryFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SyncChangedDateSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ScanDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PathDirectoryFieldSetting { get; set; }
    }
}