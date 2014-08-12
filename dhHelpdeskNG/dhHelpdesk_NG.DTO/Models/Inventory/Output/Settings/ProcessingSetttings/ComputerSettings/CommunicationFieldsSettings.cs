namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(ProcessingFieldSetting networkAdapterFieldSetting, ProcessingFieldSetting ipAddressFieldSetting, ProcessingFieldSetting macAddressFieldSetting, ProcessingFieldSetting rasFieldSetting, ProcessingFieldSetting novellClientFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
            this.RASFieldSetting = rasFieldSetting;
            this.NovellClientFieldSetting = novellClientFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting IPAddressFieldSetting { get; set; }
        
        [NotNull]
        public ProcessingFieldSetting MacAddressFieldSetting { get; set; }
        
        [NotNull]
        public ProcessingFieldSetting RASFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting NovellClientFieldSetting { get; set; }
    }
}