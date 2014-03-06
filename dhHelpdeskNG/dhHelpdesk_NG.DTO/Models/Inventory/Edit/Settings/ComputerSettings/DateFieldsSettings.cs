namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DateFieldsSettings
    {
        public DateFieldsSettings(FieldSetting createdDateFieldSetting, FieldSetting changedDateFieldSetting, FieldSetting syncChangedDateSetting, FieldSetting scanDateFieldSetting, FieldSetting pathDirectoryFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangedDateSetting = syncChangedDateSetting;
            this.ScanDateFieldSetting = scanDateFieldSetting;
            this.PathDirectoryFieldSetting = pathDirectoryFieldSetting;
        }

        [NotNull]
        public FieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SyncChangedDateSetting { get; set; }

        [NotNull]
        public FieldSetting ScanDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PathDirectoryFieldSetting { get; set; }
    }
}