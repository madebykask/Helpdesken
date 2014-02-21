namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(FieldSettingOverview networkAdapterFieldSetting, FieldSettingOverview ipAddressFieldSetting, FieldSettingOverview macAddressFieldSetting, FieldSettingOverview rasFieldSetting, FieldSettingOverview novellClientFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
            this.RASFieldSetting = rasFieldSetting;
            this.NovellClientFieldSetting = novellClientFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview IPAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview MacAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview RASFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview NovellClientFieldSetting { get; set; }
    }
}