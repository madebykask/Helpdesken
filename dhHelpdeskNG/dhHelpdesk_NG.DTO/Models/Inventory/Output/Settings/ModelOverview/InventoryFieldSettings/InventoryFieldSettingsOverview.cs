namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsOverview
    {
        public InventoryFieldSettingsOverview(DefaultFieldSettings defaultSettings)
        {
            this.DefaultSettings = defaultSettings;
        }

        [NotNull]
        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}