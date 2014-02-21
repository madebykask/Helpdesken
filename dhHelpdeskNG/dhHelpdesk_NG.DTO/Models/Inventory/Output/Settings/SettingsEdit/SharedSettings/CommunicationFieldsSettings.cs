namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(FieldSetting networkAdapterFieldSetting, FieldSetting ipAddressFieldSetting, FieldSetting macAddressFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
        }

        [NotNull]
        public FieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public FieldSetting IPAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSetting MacAddressFieldSetting { get; set; }
    }
}