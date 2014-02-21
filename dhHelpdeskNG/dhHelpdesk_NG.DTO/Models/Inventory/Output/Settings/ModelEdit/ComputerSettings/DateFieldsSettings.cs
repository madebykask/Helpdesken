namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DateFieldsSettings
    {
        public DateFieldsSettings(ModelEditFieldSetting createdDateFieldSetting, ModelEditFieldSetting changedDateFieldSetting, ModelEditFieldSetting syncChangedDateSetting, ModelEditFieldSetting scanDateFieldSetting, ModelEditFieldSetting pathDirectoryFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangedDateSetting = syncChangedDateSetting;
            this.ScanDateFieldSetting = scanDateFieldSetting;
            this.PathDirectoryFieldSetting = pathDirectoryFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SyncChangedDateSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ScanDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PathDirectoryFieldSetting { get; set; }
    }
}