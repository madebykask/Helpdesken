namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsSettings
    {
        public StorageFieldsSettings(FieldSetting capasityFieldSetting)
        {
            this.CapasityFieldSetting = capasityFieldSetting;
        }

        [NotNull]
        public FieldSetting CapasityFieldSetting { get; set; }
    }
}