namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsProcessing
    {
        public InventoryFieldSettingsProcessing(DefaultFieldSettings defaultSettings)
        {
            this.DefaultSettings = defaultSettings;
        }

        [NotNull]
        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}