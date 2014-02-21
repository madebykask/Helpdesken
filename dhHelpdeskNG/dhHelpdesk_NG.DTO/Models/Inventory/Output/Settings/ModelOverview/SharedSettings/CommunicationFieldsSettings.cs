namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(FieldSettingOverview networkAdapterFieldSetting, FieldSettingOverview ipAddressFieldSetting, FieldSettingOverview macAddressFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview IPAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview MacAddressFieldSetting { get; set; }
    }
}