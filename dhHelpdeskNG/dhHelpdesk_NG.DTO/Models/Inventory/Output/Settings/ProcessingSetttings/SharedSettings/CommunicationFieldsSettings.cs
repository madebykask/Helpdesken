namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(ProcessingFieldSetting networkAdapterFieldSetting, ProcessingFieldSetting ipAddressFieldSetting, ProcessingFieldSetting macAddressFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting IPAddressFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting MacAddressFieldSetting { get; set; }
    }
}