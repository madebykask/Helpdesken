namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSetting createdDateFieldSetting, FieldSetting changedDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
        }

        [NotNull]
        public FieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ChangedDateFieldSetting { get; set; }
    }
}