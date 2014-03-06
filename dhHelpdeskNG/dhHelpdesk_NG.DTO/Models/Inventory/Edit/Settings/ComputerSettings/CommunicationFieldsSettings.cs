namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(FieldSetting networkAdapterFieldSetting, FieldSetting ipAddressFieldSetting, FieldSetting macAddressFieldSetting, FieldSetting rasFieldSetting, FieldSetting novellClientFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
            this.RASFieldSetting = rasFieldSetting;
            this.NovellClientFieldSetting = novellClientFieldSetting;
        }

        [NotNull]
        public FieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public FieldSetting IPAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSetting MacAddressFieldSetting { get; set; }

        [NotNull]
        public FieldSetting RASFieldSetting { get; set; }

        [NotNull]
        public FieldSetting NovellClientFieldSetting { get; set; }
    }
}