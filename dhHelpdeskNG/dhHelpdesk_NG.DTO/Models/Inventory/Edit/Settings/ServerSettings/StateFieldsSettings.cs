namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSetting createdDateFieldSetting, FieldSetting changedDateFieldSetting, FieldSetting syncChangeDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangeDateFieldSetting = syncChangeDateFieldSetting;
        }

        [NotNull]
        public FieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SyncChangeDateFieldSetting { get; set; }
    }
}