namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsSettings
    {
        public MemoryFieldsSettings(FieldSetting ramFieldSetting)
        {
            this.RAMFieldSetting = ramFieldSetting;
        }

        [NotNull]
        public FieldSetting RAMFieldSetting { get; set; }
    }
}