namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsForModelEdit
    {
        public InventoryFieldSettingsForModelEdit(DefaultFieldSettings defaultSettings)
        {
            this.DefaultSettings = defaultSettings;
        }

        [NotNull]
        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}