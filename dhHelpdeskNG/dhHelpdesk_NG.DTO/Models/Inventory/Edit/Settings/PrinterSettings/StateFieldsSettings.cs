namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSetting createdDateFieldSetting, FieldSetting changedDateFieldSetting, FieldSetting syncDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncDateFieldSetting = syncDateFieldSetting;
        }

        [NotNull]
        public FieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SyncDateFieldSetting { get; set; }
    }
}