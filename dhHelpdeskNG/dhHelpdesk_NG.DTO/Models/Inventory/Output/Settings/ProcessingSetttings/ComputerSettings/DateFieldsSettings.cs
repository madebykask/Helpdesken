namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DateFieldsSettings
    {
        public DateFieldsSettings(ProcessingFieldSetting createdDateFieldSetting, ProcessingFieldSetting changedDateFieldSetting, ProcessingFieldSetting syncChangedDateSetting, ProcessingFieldSetting scanDateFieldSetting, ProcessingFieldSetting pathDirectoryFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangedDateSetting = syncChangedDateSetting;
            this.ScanDateFieldSetting = scanDateFieldSetting;
            this.PathDirectoryFieldSetting = pathDirectoryFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SyncChangedDateSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ScanDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PathDirectoryFieldSetting { get; set; }
    }
}