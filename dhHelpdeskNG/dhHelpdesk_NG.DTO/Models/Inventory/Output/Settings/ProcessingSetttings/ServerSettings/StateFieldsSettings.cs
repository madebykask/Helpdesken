namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ProcessingFieldSetting createdDateFieldSetting, ProcessingFieldSetting changedDateFieldSetting, ProcessingFieldSetting syncChangeDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangeDateFieldSetting = syncChangeDateFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SyncChangeDateFieldSetting { get; set; }
    }
}